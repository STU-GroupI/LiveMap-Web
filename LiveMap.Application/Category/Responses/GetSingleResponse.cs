using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Category.Responses;
public sealed record GetSingleResponse(Models.Category? Category);
