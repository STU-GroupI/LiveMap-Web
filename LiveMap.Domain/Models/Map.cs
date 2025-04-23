namespace LiveMap.Domain.Models;
public class Map
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Coordinate[] Area { get; set; }
    public required Coordinate Coordinate { get; set; }

    public virtual ICollection<PointOfInterest>? PointOfInterests { get; set; }
}