using BuildingBlocks.Core.Model;
using ECommerce.Infrastructure.Categories.Events;
using ECommerce.Infrastructure.Categories.ValueObjects;

namespace ECommerce.Infrastructure.Categories.Models;
public class Category : Aggregate<CategoryId>
{
    public Name Name { get; private set; }

    public static Category Create(CategoryId id, Name name, bool isDeleted = false)
    {
        Category category = new()
        {
            Id = id,
            Name = name,
            IsDeleted = isDeleted
        };

        CategoryCreatedDomainEvent @event = new(category.Id.Value, category.Name.Value, category.IsDeleted);

        category.AddDomainEvent(@event);

        return category;
    }
}
