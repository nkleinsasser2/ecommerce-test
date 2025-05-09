namespace ECommerce.Infrastructure.Orders.ValueObjects;

using Exceptions;

public record OrderId
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    public static OrderId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidOrderIdExceptions(value);

        return new OrderId(value);
    }

    public static implicit operator Guid(OrderId orderId) => orderId.Value;
}
