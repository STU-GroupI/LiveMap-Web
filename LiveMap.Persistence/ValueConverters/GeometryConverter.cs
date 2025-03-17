using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.ValueConverters;
public class GeometryConverter<T> : ValueConverter<T, T> where T : Geometry
{
    public GeometryConverter() : base(
        v => v, 
        v => SetSrid(v)
    )
    { }

    private static T SetSrid(T geometry) => geometry switch
    {
        Polygon p => new Polygon(p.ring),
        Point p => new Point(p.Coordinate),
        _ => throw new ArgumentException($"The type of {typeof(geometry)} is not supported by GeometryConverter")
    };
}