using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;
public class Map
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Coordinate[] Area { get; set; }
    public Coordinate Coordinate { get; set; }
    public int WidthInMeters { get; set; }
    public int LengthInMeters { get; set; }

    public virtual ICollection<PointOfInterest> PointOfInterests { get; set; }
}