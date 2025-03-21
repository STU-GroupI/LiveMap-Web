using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.Extensions;
public static class NetTopologyExtensions
{
    public static Domain.Models.Coordinate ToDomainCoordinate(this Point coordinate) => new(coordinate.X, coordinate.Y);

    public static Domain.Models.Coordinate ToDomainCoordinate(this Coordinate coordinate) => new(coordinate.X, coordinate.Y);

    public static Domain.Models.Coordinate[] ToDomainCoordinates(this Polygon polygon) => polygon.Coordinates
        .Select(x => x.ToDomainCoordinate())
        .ToArray();
}
