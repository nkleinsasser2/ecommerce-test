
using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Inventories.Enums;
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Inventories.ValueObjects;
using ECommerce.Infrastructure.Products.ValueObjects;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Inventories.Features.AddingProductToInventory;
public record AddProductToInventory(Guid InventoryId, Guid ProductId, int Quantity) : ICommand<AddProductToInventoryResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record AddProductToInventoryResult(Guid Id);

public record AddProductToInventoryRequestDto(Guid InventoryId, Guid ProductId, int Quantity);

public record AddProductToInventoryResponseDto(Guid Id);

public class AddProductToInventoryEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        _ = builder.MapPost($"{EndpointConfig.BaseApiPath}/inventory/add-product-to-inventory", async (
                AddProductToInventoryRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                AddProductToInventory command = mapper.Map<AddProductToInventory>(request);

                AddProductToInventoryResult result = await mediator.Send(command, cancellationToken);

                AddProductToInventoryResponseDto response = mapper.Map<AddProductToInventoryResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Add Product To Inventory")
            .WithSummary("Add Product To Inventory")
            .WithDescription("Add Product To Inventory")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces<AddProductToInventoryResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class AddProductToInventoryValidator : AbstractValidator<AddProductToInventory>
{
    public AddProductToInventoryValidator()
    {
        _ = RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId must be not empty");
        _ = RuleFor(x => x.InventoryId).NotEmpty().WithMessage("InventoryId must be not empty");
        _ = RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class AddProductToInventoryHandler : ICommandHandler<AddProductToInventory, AddProductToInventoryResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public AddProductToInventoryHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<AddProductToInventoryResult> Handle(AddProductToInventory request, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(request, nameof(request));

        InventoryItems? productInventoryItems = await _eCommerceDbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == ProductId.Of(request.ProductId) && x.Status == ProductStatus.InStock,
                cancellationToken: cancellationToken);

        if (productInventoryItems is not null)
        {
            productInventoryItems.UpdateProductToInventory(productInventoryItems.Id,
                InventoryId.Of(request.InventoryId),
                ProductId.Of(request.ProductId),
                Quantity.Of(request.Quantity + productInventoryItems.Quantity.Value));

            _ = _eCommerceDbContext.InventoryItems.Update(productInventoryItems);
            return new AddProductToInventoryResult(productInventoryItems.Id.Value);
        }

        InventoryItems productInventoryItemsEntity = InventoryItems.AddProductToInventory(InventoryItemsId.Of(request.Id),
            InventoryId.Of(request.InventoryId),
            ProductId.Of(request.ProductId),
            Quantity.Of(request.Quantity));

        _ = await _eCommerceDbContext.InventoryItems.AddAsync(productInventoryItemsEntity, cancellationToken);

        return new AddProductToInventoryResult(productInventoryItemsEntity.Id.Value);
    }
}
