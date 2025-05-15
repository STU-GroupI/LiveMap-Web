namespace LiveMap.Application.Category.Responses;
using Domain.Models;

public sealed record UpdateSingleResponse(string OldName, string NewName, string iconName, bool Success);