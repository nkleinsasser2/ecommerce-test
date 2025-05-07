namespace BuildingBlocks.Core.Model;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    int? DeletedBy { get; set; }
} 