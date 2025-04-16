using LiveMap.Domain.Models;
using LiveMap.Persistence.Extensions;
using NetTopologySuite.Geometries;

namespace LiveMap.Persistence.DbModels;

public class SqlMap
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Polygon Border { get; set; }
    public required Point Position { get; set; }

    public virtual required ICollection<SqlPointOfInterest> PointOfInterests { get; set; }
}
