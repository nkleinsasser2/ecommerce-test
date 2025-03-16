
using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Inventories.Enums;
using ECommerce.Infrastructure.Inventories.Events;
using ECommerce.Infrastructure.Inventories.Exceptions;
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

namespace ECommerce.Inventories.Features.DamagingProduct;
public record DamageProduct(Guid ProductId, int Quantity) : ICommand;

public record DamageProductRequestDto(Guid ProductId, int Quantity);

public class DamageProductEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        _ = builder.MapPost($"{EndpointConfig.BaseApiPath}/inventory/damage-product", async (
                DamageProductRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                DamageProduct command = mapper.Map<DamageProduct>(request);

                _ = await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .WithName("Damage Product")
            .WithSummary("Damage Product")
            .WithDescription("Damage Product")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class DamageProductValidator : AbstractValidator<DamageProduct>
{
    public DamageProductValidator()
    {
        _ = RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId must be not empty");
        _ = RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class DamageProductHandler : ICommandHandler<DamageProduct>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public DamageProductHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<Unit> Handle(DamageProduct request, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(request, nameof(request));

        InventoryItems? productsInventoryItems = await _eCommerceDbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == ProductId.Of(request.ProductId) && x.Status == ProductStatus.InStock,
                cancellationToken: cancellationToken);

        if (productsInventoryItems is null)
        {
            throw new ProductNotExistToInventoryException();
        }

        if (request.Quantity > productsInventoryItems.Quantity.Value)
        {
            throw new OutOfRangeQuantityException(request.Quantity, productsInventoryItems.Quantity.Value);
        }

        productsInventoryItems.DamageProduct(productsInventoryItems.Id, productsInventoryItems.InventoryId,
            ProductId.Of(request.ProductId), Quantity.Of(request.Quantity));

        _ = _eCommerceDbContext.InventoryItems.Update(productsInventoryItems);

        return Unit.Value;
    }

    public class ProductDamagedDomainEventHandler : INotificationHandler<ProductDamagedDomainEvent>
    {
        private readonly ECommerceDbContext _eCommerceDbContext;

        public ProductDamagedDomainEventHandler(ECommerceDbContext eCommerceDbContext)
        {
            _eCommerceDbContext = eCommerceDbContext;
        }

        public async Task Handle(ProductDamagedDomainEvent notification, CancellationToken cancellationToken)
        {
            _ = Guard.Against.Null(notification, nameof(notification));

            InventoryItems productInventoryItemsEntity = InventoryItems.AddProductToInventory(
                InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(notification.InventoryId),
                ProductId.Of(notification.ProductId),
                Quantity.Of(notification.Quantity),
                ProductStatus.Damaged);

            _ = await _eCommerceDbContext.InventoryItems.AddAsync(productInventoryItemsEntity, cancellationToken);
            await _eCommerceDbContext.ExecuteTransactionalAsync(cancellationToken);
        }
    }
}
