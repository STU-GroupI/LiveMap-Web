namespace LiveMap.Domain.Models;
public sealed class SuggestedPointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Coordinate Coordinate { get; set; }

    public string? CategoryName { get; set; }
    public required Category Category { get; set; }

    public string? StatusName { get; set; }
    public required PointOfInterestStatus Status { get; set; }

    public required Guid MapId { get; set; }
    public required Map Map { get; set; }

    public Guid? RFCId { get; set; }
    public required RequestForChange RFC { get; set; }
}