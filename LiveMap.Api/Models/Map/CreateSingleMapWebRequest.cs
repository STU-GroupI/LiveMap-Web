using LiveMap.Domain.Models;

namespace LiveMap.Api.Models.Map;

public sealed record CreateSingleMapWebRequest(
    string Name,
    Coordinate[] Area,
    Coordinate[] Bounds,
    string? ImageUrl = null
);
