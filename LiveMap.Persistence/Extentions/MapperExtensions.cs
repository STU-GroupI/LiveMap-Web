using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        PointOfInterest data = new()
        {
            Id = poi.Id,
            Category = poi.Category,
            CategoryName = poi.CategoryName,
            Coordinate = poi.Position.Coordinate.ToDomainCoordinate(),
            Description = poi.Description,
            MapId = poi.MapId,
            Map = poi.Map?.ToDomainMap() ?? null,
            Status = poi.Status,
            Title = poi.Title,
            StatusName = poi.StatusName,
        };
        data.OpeningHours = poi.OpeningHours?.Select(oh => oh.ToOpeningHours(data)).ToList() ?? [];
        return data;
    }

    public static OpeningHours ToOpeningHours(this SqlOpeningHours oh, PointOfInterest poi)
    {
        return new()
        {
            Id = oh.Id,
            DayOfWeek = oh.DayOfWeek,
            Start = oh.Start,
            End = oh.End,
            PoiId = oh.PoiId,
            Poi = poi,
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
            IsWheelchairAccessible = suggestedPoi.IsWheelchairAccessible,

            CategoryName = suggestedPoi.CategoryName,
            Category = suggestedPoi?.Category,

            MapId = suggestedPoi.MapId,
            Map = suggestedPoi.Map?.ToDomainMap(),

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
            ApprovalStatus = requestForChange.ApprovalStatus,
            PoiId = requestForChange.PoiId,
            Poi = requestForChange.Poi?.ToDomainPointOfInterest(),
            SubmittedOn = requestForChange.SubmittedOn,
            ApprovedOn = requestForChange.ApprovedOn ?? default,
            SuggestedPoiId = requestForChange.SuggestedPoiId,
            Message = requestForChange.Message
        };

        // If the suggested poi is not given, then we know it does not have its RFC yet. When converting it to its domain format, we must
        // explicitly tell it to use our RFC value instead of the one that it will generate, or we will get a loop.
        value.SuggestedPoi = suggestedPoi is null ? requestForChange.SuggestedPoi?.ToDomainSuggestedPointOfInterest(value) : suggestedPoi;

        return value;
    }

    public static SqlRequestForChange ToSqlRequestForChange(this RequestForChange requestForChange)
    {
        return new SqlRequestForChange
        {
            Id = requestForChange.Id,
            ApprovalStatus = requestForChange.ApprovalStatus,
            ApprovedOn = requestForChange.ApprovedOn,
            SuggestedPoiId = requestForChange.SuggestedPoiId,
            SubmittedOn = requestForChange.SubmittedOn,
            PoiId = requestForChange.PoiId,
            Message = requestForChange.Message,
            ApprovalStatusProp = new ApprovalStatus { Status = requestForChange.ApprovalStatus }
        };
    }


    public static SqlSuggestedPointOfInterest ToSqlSuggestedPointOfInterest(this SuggestedPointOfInterest suggestedPoi)
    {
        return new SqlSuggestedPointOfInterest
        {
            Id = default,
            Title = suggestedPoi.Title,
            Description = suggestedPoi.Description,
            CategoryName = suggestedPoi.CategoryName,
            IsWheelchairAccessible = suggestedPoi.IsWheelchairAccessible,
            Position = suggestedPoi.Coordinate.ToSqlPoint(),
            MapId = suggestedPoi.MapId,
            RFC = null,
            Category = null!,
            Map = null!
        };
    }
}