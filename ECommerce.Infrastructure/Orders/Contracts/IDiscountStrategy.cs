namespace ECommerce.Infrastructure.Orders.Contracts;

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal amount);
}
