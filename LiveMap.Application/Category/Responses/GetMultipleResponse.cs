using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Category.Responses;
public sealed record GetMultipleResponse(Models.Category[] Categories);

