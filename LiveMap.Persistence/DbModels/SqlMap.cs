﻿using NetTopologySuite.Geometries;

namespace LiveMap.Persistence.DbModels;

public class SqlMap
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Polygon Border { get; set; }
    public required Polygon Bounds { get; set; }
    public string? ImageUrl { get; set; }

    public virtual ICollection<SqlPointOfInterest> PointOfInterests { get; set; } = [];
}
