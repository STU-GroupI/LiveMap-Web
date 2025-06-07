namespace LiveMap.Application.Category.Requests;

public sealed record UpdateSingleRequest(string oldName, string newName, string iconName);
