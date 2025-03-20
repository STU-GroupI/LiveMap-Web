using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetTopologySuite.Geometries.Utilities.GeometryMapper;

namespace LiveMap.Persistence.Extentions;
public static class MapperExtentions
{
    public static Map ToMap(this SqlMap map)
    {
        return new()
        {
            Id = map.Id,
            Name = map.Name,
            Coordinate = map.Position.ToDomainCoordinate(),
            PointOfInterests = new List<PointOfInterest>(),
            Area = map.Border.ToDomainCoordinates(),
        };
    }

    public static PointOfInterest ToPointOfInterest(this SqlPointOfInterest poi)
    {
        return new()
        {
            Id = poi.Id,
            Category = poi.Category,
            CategoryName = poi.CategoryName,
            Coordinate = poi.Position.Coordinate.ToDomainCoordinate(),
            Description = poi.Description,
            MapId = poi.MapId,
            Map = poi.Map?.ToMap() ?? null,
            Status = poi.Status,
            Title = poi.Title,
            StatusName = poi.StatusName
        };
    }
}