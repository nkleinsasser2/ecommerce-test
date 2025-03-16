namespace ECommerce.Infrastructure.Inventories.Dtos;

using ECommerce.Infrastructure.Inventories.Enums;
using Enums;

public record InventoryItemsDto(Guid Id, Guid InventoryId, Guid ProductId, int Quantity, ProductStatus Status);
