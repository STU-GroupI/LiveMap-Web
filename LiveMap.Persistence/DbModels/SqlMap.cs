using LiveMap.Domain.Models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.DbModels;

public class SqlMap
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Polygon Edge { get; set; }
    public Point Coordinate { get; set; }
    public int WidthInMeters { get; set; }
    public int LengthInMeters { get; set; }

    public virtual ICollection<SqlPointOfInterest> PointOfInterests { get; set; }
}
