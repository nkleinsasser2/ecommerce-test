using BuildingBlocks.Core.Event;

namespace ECommerce.Infrastructure.Products.Events;


public record ProductCreatedDomainEvent(Guid Id, string Name, string Barcode, bool Weighted, Guid CategoryId,
    decimal Price, decimal ProfitMargin, decimal NetPrice,
    string Description, bool IsDeleted) : IDomainEvent;
