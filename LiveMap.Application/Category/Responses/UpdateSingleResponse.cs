namespace LiveMap.Application.Category.Responses;
using Domain.Models;

public sealed record UpdateSingleResponse(string oldname, string newname, bool Success);