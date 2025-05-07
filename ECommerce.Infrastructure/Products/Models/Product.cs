using ECommerce.Infrastructure.Products.ValueObjects;

namespace ECommerce.Infrastructure.Products.Models;

using BuildingBlocks.Core.Model;
using Categories.Models;
using Categories.ValueObjects;
using ECommerce.Infrastructure.Categories.Models;
using ECommerce.Infrastructure.Categories.ValueObjects;
using ECommerce.Infrastructure.Products.Events;
using JetBrains.Annotations;
using ValueObjects;
using Name = Name;

public class Product : Aggregate<ProductId>
{
    private NetPrice _netPrice;
    public Name Name { get; private set; }
    public Barcode Barcode { get; private set; }
    public Description? Description { get; private set; }
    public Category? Category { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public bool IsBreakable { get; private set; }
    public Price Price { get; private set; }
    public NetPrice NetPrice {get; private set;}

    public ProfitMargin? ProfitMargin { get; set; }

    public static Product Create(ProductId id, Name name, Barcode barcode, bool isBreakable,
        CategoryId categoryId,
        Price price,
        ProfitMargin profitMargin,
        Description? description = null, bool isDeleted = false)
    {
        var product = new Product
        {
            Id = id,
            Name = name,
            Barcode = barcode,
            IsBreakable = isBreakable,
            CategoryId = categoryId,
            Description = description,
            Price = price,
            ProfitMargin = profitMargin,
            IsDeleted = isDeleted,
            NetPrice = NetPrice.Of(price.Value + profitMargin?.Value ?? 0),
        };

        var @event = new ProductCreatedDomainEvent(product.Id, product.Name, product.Barcode,
            product.IsBreakable, product.CategoryId, product.Price, product.ProfitMargin, product.NetPrice,
            product.Description, product.IsDeleted);

        product.AddDomainEvent(@event);

        return product;
    }
}
