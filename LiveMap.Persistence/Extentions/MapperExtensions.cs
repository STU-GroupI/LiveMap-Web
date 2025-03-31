﻿using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetTopologySuite.Geometries.Utilities.GeometryMapper;

namespace LiveMap.Persistence.Extensions;
public static class MapperExtensions
{
    public static Map ToDomainMap(this SqlMap map)
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

    public static PointOfInterest ToDomainPointOfInterest(this SqlPointOfInterest poi)
    {
        return new()
        {
            Id = poi.Id,
            Category = poi.Category,
            CategoryName = poi.CategoryName,
            Coordinate = poi.Position.Coordinate.ToDomainCoordinate(),
            Description = poi.Description,
            MapId = poi.MapId,
            Map = poi.Map.ToDomainMap() ?? null,
            Status = poi.Status,
            Title = poi.Title,
            StatusName = poi.StatusName
        };
    }
    
    public static SuggestedPointOfInterest ToDomainSuggestedPointOfInterest(this SqlSuggestedPointOfInterest suggestedPoi)
    {
        return suggestedPoi.ToDomainSuggestedPointOfInterest(null);
    }
    public static SuggestedPointOfInterest ToDomainSuggestedPointOfInterest(this SqlSuggestedPointOfInterest suggestedPoi, RequestForChange? rfc)
    {
        SuggestedPointOfInterest value = new()
        {
            Id = suggestedPoi.Id,
            Title = suggestedPoi.Title,

            CategoryName = suggestedPoi.CategoryName,
            Category = suggestedPoi.Category,

            MapId = suggestedPoi.MapId,
            Map = suggestedPoi.Map.ToDomainMap(),

            StatusName = suggestedPoi.StatusName,
            Status = suggestedPoi.Status,

            Coordinate = suggestedPoi.Position.ToDomainCoordinate(),
            Description = suggestedPoi.Description,

            RFCId = suggestedPoi.RFCId,
            RFC = null!
        };

        // If the rfc is not given, then we know it does not have its suggested PoI yet. When converting it to its domain format, we must
        // explicitly tell it to use our PoI value instead of the one that it will generate, or we will get a loop.
        value.RFC = rfc is null ? suggestedPoi.RFC.ToDomainRequestForChange(value) : rfc;

        return value;
    }

    public static RequestForChange ToDomainRequestForChange(this SqlRequestForChange requestForChange)
    {
        return requestForChange.ToDomainRequestForChange(null);
    }
    public static RequestForChange ToDomainRequestForChange(this SqlRequestForChange requestForChange, SuggestedPointOfInterest? suggestedPoi)
    {
        RequestForChange value = new()
        {
            Id = requestForChange.Id,
            Status = requestForChange.StatusProp,
            PoiId = requestForChange.PoiId,
            Poi = requestForChange.Poi?.ToDomainPointOfInterest(),
            SuggestedPoiId = requestForChange.SuggestedPoiId,
        };

        // If the suggested poi is not given, then we know it does not have its RFC yet. When converting it to its domain format, we must
        // explicitly tell it to use our RFC value instead of the one that it will generate, or we will get a loop.
        value.SuggestedPoi = suggestedPoi is null ? requestForChange.SuggestedPoi?.ToDomainSuggestedPointOfInterest(value) : suggestedPoi;

        return value;
    }
}