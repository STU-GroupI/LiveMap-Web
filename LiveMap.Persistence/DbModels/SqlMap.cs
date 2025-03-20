using LiveMap.Domain.Models;
using LiveMap.Persistence.Extentions;
using NetTopologySuite.Geometries;

namespace LiveMap.Persistence.DbModels;

public class SqlMap
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Polygon Border { get; set; }
    public Point Position { get; set; }

    public virtual ICollection<SqlPointOfInterest> PointOfInterests { get; set; }
}
