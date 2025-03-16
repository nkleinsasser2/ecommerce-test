using ECommerce.Infrastructure.Orders.Models;

namespace ECommerce.Infrastructure.Orders.Contracts.Strategies.Shipment;
public class ExpressPostStrategy : IShipmentStrategy
{
    private const decimal shipmentExpressPostPrice = 500;

    public List<OrderItem> GetOrderItemsToShip(IList<OrderItem> orderItems)
    {
        return orderItems.Where(p => p.Product.IsBreakable).ToList();
    }

    public decimal GetShipmentPriceُ() => shipmentExpressPostPrice;
}
