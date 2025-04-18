﻿using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IMapService
{
    Task<BackendApiHttpResponse<DomainModels.Map>> Get(Guid id);
    Task<BackendApiHttpResponse<DomainModels.Map[]>> Get(int? skip, int? take);
    Task<BackendApiHttpResponse<DomainModels.Map>> CreateSingle(DomainModels.Map poi);
    Task<BackendApiHttpResponse<DomainModels.Map>> UpdateSingle(DomainModels.Map poi);
    Task<BackendApiHttpResponse> Delete(DomainModels.Map poi);
}