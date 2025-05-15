using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Text.Json;

namespace LiveMap.Persistence.Extensions;
public static class NetTopologyExtensions
{
    public static Domain.Models.Coordinate ToDomainCoordinate(this Point coordinate) => new(coordinate.X, coordinate.Y);

    public static Domain.Models.Coordinate ToDomainCoordinate(this Coordinate coordinate) => new(coordinate.X, coordinate.Y);

    public static Domain.Models.Coordinate[] ToDomainCoordinates(this Polygon polygon) => polygon.Coordinates
        .Select(x => x.ToDomainCoordinate())
        .ToArray();

    public static string ToJsonCoordinates(this Domain.Models.Coordinate[] coordinates) => JsonSerializer.Serialize(coordinates);

    public static Point ToSqlPoint(this Domain.Models.Coordinate coordinate)
    {
        return new Point(new Coordinate(coordinate.Longitude, coordinate.Latitude))
        {
            SRID = 4326
        };
    }

    public static Polygon ToPolygon(this Domain.Models.Coordinate[] domainCoordinates)
    {
        if (domainCoordinates == null || domainCoordinates.Length < 3)
        {
            throw new ArgumentException("A polygon must have at least three distinct points.");
        }

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        // Convert DomainCoordinate to NTS Coordinate
        var coordinates = domainCoordinates
            .Select(dc => new Coordinate(dc.Longitude, dc.Latitude))
            .ToList();

        // Ensure the first and last coordinate are the same to close the polygon
        if (!coordinates[0].Equals2D(coordinates[^1]))
        {
            coordinates.Add(coordinates[0]);  // Close the ring
        }

        var linearRing = new LinearRing(coordinates.ToArray());
        return geometryFactory.CreatePolygon(linearRing);
    }
}
