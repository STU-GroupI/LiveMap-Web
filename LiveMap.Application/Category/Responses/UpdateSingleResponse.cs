namespace LiveMap.Application.Category.Responses;
public sealed record UpdateSingleResponse(string OldName, string NewName, string iconName, bool Success);