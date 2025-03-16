
using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Categories.ValueObjects;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Products.Exceptions;
using ECommerce.Infrastructure.Products.Models;
using ECommerce.Infrastructure.Products.ValueObjects;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Products.Features.CreatingProduct;
public record CreateProduct(string Name, string Barcode, bool Weighted,
    Guid CategoryId, decimal Price, decimal ProfitMargin, string Description) : ICommand<CreateProductResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateProductResult(Guid Id);

public record CreateProductRequestDto(string Name, string Barcode, bool Weighted,
    Guid CategoryId, decimal Price, decimal ProfitMargin, string Description);

public record CreateProductResponseDto(Guid Id);

public class CreateProductEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        _ = builder.MapPost($"{EndpointConfig.BaseApiPath}/catalog/product", async (CreateProductRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                CreateProduct command = mapper.Map<CreateProduct>(request);

                CreateProductResult result = await mediator.Send(command, cancellationToken);

                CreateProductResponseDto response = new(result.Id);

                return Results.Ok(response);
            })
            .WithName("Create Product")
            .WithSummary("Create Product")
            .WithDescription("Create Product")
            .WithApiVersionSet(builder.NewApiVersionSet("Catalog").Build())
            .Produces<CreateProductResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {
        _ = RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barcode must be not empty");
        _ = RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be not empty");
        _ = RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId must be not empty");
        _ = RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be equal or greater than 0");
        _ = RuleFor(x => x.ProfitMargin).GreaterThanOrEqualTo(0)
            .WithMessage("ProfitMargin must be equal or greater than 0");
    }
}

public class CreateProductHandler : ICommandHandler<CreateProduct, CreateProductResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public CreateProductHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<CreateProductResult> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(request, nameof(request));

        Product? product = await _eCommerceDbContext.Products.SingleOrDefaultAsync(x => x.Id == ProductId.Of(request.Id),
            cancellationToken);

        if (product is not null)
        {
            throw new ProductAlreadyExistException();
        }

        Product productEntity = Product.Create(ProductId.Of(request.Id), Infrastructure.Products.ValueObjects.Name.Of(request.Name),
            Barcode.Of(request.Barcode), request.Weighted, CategoryId.Of(request.CategoryId), Price.Of(request.Price),
            ProfitMargin.Of(request.ProfitMargin)
            , Description.Of(request.Description));

        Product newProduct = (await _eCommerceDbContext.Products.AddAsync(productEntity, cancellationToken)).Entity;

        return new CreateProductResult(newProduct.Id.Value);
    }
}
