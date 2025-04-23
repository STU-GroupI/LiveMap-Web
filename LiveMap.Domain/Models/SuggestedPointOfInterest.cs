namespace LiveMap.Domain.Models;

public sealed class SuggestedPointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Coordinate Coordinate { get; set; }

    public required string CategoryName { get; set; }
    public Category? Category { get; set; }

    public required Guid MapId { get; set; }
    public Map? Map { get; set; }

    public required bool IsWheelchairAccessible { get; set; } = false;

    public Guid? RFCId { get; set; }
    public RequestForChange? RFC { get; set; }
}