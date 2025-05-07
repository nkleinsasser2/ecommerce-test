using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Web;
using ECommerce.Infrastructure.Customers.ValueObjects;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Inventories.Enums;
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Orders.Dtos;
using ECommerce.Infrastructure.Orders.Enums;
using ECommerce.Infrastructure.Orders.Exceptions;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Orders.ValueObjects;
using ECommerce.Infrastructure.Products.ValueObjects;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders.Api.Features.RegisteringNewOrder;
public record RegisterNewOrder(Guid CustomerId,
    IEnumerable<ItemDto> Items, DiscountType DiscountType, decimal DiscountValue, DateTime? OrderDate = null) : ICommand<RegisterNewOrderResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record RegisterNewOrderRequestDto(Guid CustomerId,
    IEnumerable<ItemDto> Items, DiscountType DiscountType, decimal DiscountValue, DateTime? OrderDate = null);

public record RegisterNewOrderResult(Guid Id, Guid CustomerId, string Status, decimal TotalPrice,
    DateTime OrderDate, IEnumerable<OrderItemDto> RegularOrderItems, IEnumerable<OrderItemDto> ExpressOrderItems,
    string DiscountType, decimal DiscountValue);

public record RegisterNewOrderResponseDto(Guid Id, Guid CustomerId, string Status, decimal TotalPrice,
    DateTime OrderDate, IEnumerable<OrderItemDto> RegularOrderItems, IEnumerable<OrderItemDto> ExpressOrderItems,
    string DiscountType, decimal DiscountValue);

public class RegisterNewOrderEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup("api/order");

        group.MapPost("register-new-order", async (
                RegisterNewOrderRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                RegisterNewOrder command = mapper.Map<RegisterNewOrder>(request);
                RegisterNewOrderResult result = await mediator.Send(command, cancellationToken);
                RegisterNewOrderResponseDto response = mapper.Map<RegisterNewOrderResponseDto>(result);
                return Results.Ok(response);
            })
            .WithName("Register New Order")
            .WithSummary("Register New Order")
            .WithDescription("Register New Order")
            .Produces<RegisterNewOrderResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        return builder;
    }
}

public class RegisterNewOrderValidator : AbstractValidator<RegisterNewOrder>
{
    public RegisterNewOrderValidator()
    {
        _ = RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId must be not empty");
        _ = RuleFor(x => x.Items).NotEmpty().WithMessage("Items must be not empty");
        _ = RuleFor(x => x.Items.Count()).GreaterThan(0).WithMessage("Items must be greater than 0");
        _ = RuleFor(x => x.DiscountValue).GreaterThanOrEqualTo(0)
            .WithMessage("DiscountValue must be equal or greater than 0");

        _ = RuleFor(x => x.DiscountType).Must(p => (p.GetType().IsEnum &&
                                                p == DiscountType.None) ||
                                               p == DiscountType.AmountDiscount ||
                                               p == DiscountType.PercentageDiscount)
            .WithMessage("Status must be None, AmountDiscount or PercentageDiscount");
    }
}

public class RegisterNewOrderHandler : ICommandHandler<RegisterNewOrder, RegisterNewOrderResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public RegisterNewOrderHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<RegisterNewOrderResult> Handle(RegisterNewOrder request, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(request, nameof(request));

        Infrastructure.Customers.Models.Customer? customer = await _eCommerceDbContext.Customers.FirstOrDefaultAsync(
            x => x.Id == CustomerId.Of(request.CustomerId),
            cancellationToken: cancellationToken);

        if (customer is null)
        {
            throw new CustomerNotExistException();
        }

        List<InventoryItems> inventoryItems = [];
        List<OrderItem> orderItems = [];

        foreach (ItemDto orderItem in request.Items)
        {
            InventoryItems? existItem =
                await _eCommerceDbContext.InventoryItems
                    .Include(i => i.Product)
                    .FirstOrDefaultAsync(x =>
                        x.ProductId == ProductId.Of(orderItem.ProductId) && 
                        x.Status == ProductStatus.InStock &&
                        x.Quantity.Value >= orderItem.Quantity && 
                        !x.IsDeleted,
                        cancellationToken: cancellationToken);

            if (existItem is null)
            {
                throw new OrderItemNotExistInInventoryException(orderItem.ProductId, orderItem.Quantity);
            }

            inventoryItems.Add(existItem);
        }

        Order order = Order.Create(
            OrderId.Of(request.Id), 
            customer, 
            request.DiscountType, 
            request.DiscountValue, 
            OrderDate.Of(request.OrderDate ?? DateTime.Now));

        foreach (var (item, inventoryItem) in request.Items.Zip(inventoryItems))
        {
            var orderItem = OrderItem.Create(
                OrderItemId.Of(NewId.NextGuid()),
                order.Id,
                inventoryItem.ProductId,
                Quantity.Of(item.Quantity));

            // Set audit fields
            orderItem.CreatedAt = DateTime.UtcNow;
            orderItem.CreatedBy = 1; // System user
            orderItem.LastModified = DateTime.UtcNow;
            orderItem.LastModifiedBy = 1; // System user
            orderItem.Version = 1;
            orderItem.IsDeleted = false;

            orderItems.Add(orderItem);
        }

        order.AddItems(orderItems);
        order.CalculateTotalPrice();

        (IEnumerable<OrderItemDto> ExpressShipmentItems, IEnumerable<OrderItemDto> RegularShipmentItems) = order.ApplyShipment();
        order.ApplyDiscount(request.DiscountType, request.DiscountValue);

        await _eCommerceDbContext.Orders.AddAsync(order, cancellationToken);
        await _eCommerceDbContext.OrderItems.AddRangeAsync(orderItems, cancellationToken);

        // Save changes to persist the order and order items
        await _eCommerceDbContext.SaveChangesAsync(cancellationToken);

        return new RegisterNewOrderResult(
            order.Id.Value, 
            customer.Id.Value, 
            order.Status.ToString(), 
            order.TotalPrice.Value,
            order.OrderDate, 
            RegularShipmentItems, 
            ExpressShipmentItems,
            request.DiscountType.ToString(), 
            request.DiscountValue);
    }
}
