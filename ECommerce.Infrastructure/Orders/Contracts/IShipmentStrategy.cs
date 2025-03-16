namespace ECommerce.Infrastructure.Orders.Contracts;

using Models;
using ValueObjects;

public interface IShipmentStrategy
{
    List<OrderItem> GetOrderItemsToShip(IList<OrderItem> orderItems);
    decimal GetShipmentPriceُ();
}
