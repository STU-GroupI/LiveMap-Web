namespace LiveMap.Domain.Models;
public class Map
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Coordinate[] Area { get; set; }
    public Coordinate Coordinate { get; set; }

    public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
}