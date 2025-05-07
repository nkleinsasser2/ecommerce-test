namespace BuildingBlocks.Core.Model;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string? DeletedBy { get; set; }
    DateTime? LastModified { get; set; }
    string? LastModifiedBy { get; set; }
    long Version { get; set; }
} 