
using AutoMapper;
using ECommerce.Infrastructure.Orders.Dtos;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Orders.Api.Features.RegisteringNewOrder;

namespace ECommerce.Orders.Api.Features;
public class OrderMappings : Profile
{
    public OrderMappings()
    {
        _ = CreateMap<RegisterNewOrderRequestDto, RegisterNewOrder>();
        _ = CreateMap<OrderItem, OrderItemDto>();
        _ = CreateMap<OrderItemDto, OrderItem>();
        _ = CreateMap<RegisterNewOrderResult, RegisterNewOrderResponseDto>();
    }
}

