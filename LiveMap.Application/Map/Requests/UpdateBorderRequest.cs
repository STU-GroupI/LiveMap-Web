using LiveMap.Domain.Models;

namespace LiveMap.Application.Map.Requests;
public sealed record UpdateBorderRequest(Guid Id, Coordinate[] Coords);