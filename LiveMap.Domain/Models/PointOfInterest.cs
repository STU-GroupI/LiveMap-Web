namespace LiveMap.Domain.Models;
public class PointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Image { get; set; }
    public required Coordinate Coordinate { get; set; }

    public required string CategoryName { get; set; }
    public Category? Category { get; set; }

    public required bool IsWheelchairAccessible { get; set; } = false;

    public required string StatusName { get; set; }
    public PointOfInterestStatus? Status { get; set; }

    public required Guid MapId { get; set; }
    public Map? Map { get; set; }

    public virtual ICollection<OpeningHours>? OpeningHours { get; set; }
}