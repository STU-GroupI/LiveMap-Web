﻿using LiveMap.Domain.Models;
namespace LiveMap.Api.Models.PointOfInterest;

public sealed record UpdateSinglePoiWebRequest(
    string Id,
    string Title,
    string Description,
    string CategoryName,
    string MapId,
    Coordinate Coordinate,
    bool IsWheelchairAccessible,
    List<OpeningHoursWebRequestModel> OpeningHours
);
