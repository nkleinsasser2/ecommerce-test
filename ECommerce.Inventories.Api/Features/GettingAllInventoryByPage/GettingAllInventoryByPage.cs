
using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.Pagination;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Inventories.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace ECommerce.Inventories.Features.GettingAllInventoryByPage;
public record GetAllInventoryByPage
    (int PageNumber, int PageSize, string Filters, string SortOrder) : IPageQuery<GetAllInventoryByPageResult>;

public record GetAllInventoryByPageRequestDto
    (int PageNumber, int PageSize, string Filters, string SortOrder) : IPageRequest;

public record GetAllInventoryByPageResult(IPageList<InventoryDto> Inventories);

public record GetAllInventoryByPageResponseDto(IPageList<InventoryDto> Inventories);

public class GetAllInventoryByPageValidator : AbstractValidator<GetAllInventoryByPage>
{
    public GetAllInventoryByPageValidator()
    {
        _ = RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page should at least greater than or equal to 1.");

        _ = RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetAllInventoryEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        _ = builder.MapGet($"{EndpointConfig.BaseApiPath}/catalog/get-all-inventory-by-page", async (
                [AsParameters] GetAllInventoryByPageRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                GetAllInventoryByPage command = mapper.Map<GetAllInventoryByPage>(request);

                GetAllInventoryByPageResult result = await mediator.Send(command, cancellationToken);

                GetAllInventoryByPageResponseDto response = mapper.Map<GetAllInventoryByPageResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Get All Inventory By Page")
            .WithSummary("Get All Inventory By Page")
            .WithDescription("Get All Inventory By Page")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces<GetAllInventoryByPageResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class GetAllInventoryByPageHandler : IRequestHandler<GetAllInventoryByPage, GetAllInventoryByPageResult>
{
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;
    private readonly ECommerceDbContext _eCommerceDbContext;

    public GetAllInventoryByPageHandler(
        ISieveProcessor sieveProcessor,
        IMapper mapper,
        ECommerceDbContext eCommerceDbContext
    )
    {
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<GetAllInventoryByPageResult> Handle(GetAllInventoryByPage request, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(request, nameof(request));

        IPageList<Infrastructure.Inventories.Models.Inventory> pageList = await _eCommerceDbContext.Inventories.AsNoTracking().ApplyPagingAsync(request, _sieveProcessor, cancellationToken);

        PageList<InventoryDto> result = _mapper.Map<PageList<InventoryDto>>(pageList);

        return new GetAllInventoryByPageResult(result);
    }
}
