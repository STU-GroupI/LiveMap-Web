namespace LiveMap.Application.Category.Responses;
using Domain.Models;

public sealed record UpdateSingleResponse(string oldName, string newName, bool Success);