using ECommerce.Infrastructure.Orders.Models;

namespace ECommerce.Infrastructure.Orders.Contracts.Strategies.Shipment;
public class RegularPostStrategy : IShipmentStrategy
{
    private const decimal shipmentRegularPostPrice = 200;

    public List<OrderItem> GetOrderItemsToShip(IList<OrderItem> orderItems)
    {
        return orderItems.Where(p => !p.Product.IsBreakable).ToList();
    }

    public decimal GetShipmentPriceُ()
    {
        return shipmentRegularPostPrice;
    }
}
