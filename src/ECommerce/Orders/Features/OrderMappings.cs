
using AutoMapper;
using ECommerce.Infrastructure.Orders.Dtos;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Orders.Features.RegisteringNewOrder;

namespace ECommerce.Orders.Features;
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

