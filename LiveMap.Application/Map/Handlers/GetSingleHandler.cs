using LiveMap.Application.Map.Persistance;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Map.Handlers;

public class GetSingleHandler : IRequestHandler<GetSingleRequest, GetSingleResponse>
{
    private readonly IMapRepository _mapRepository;

    public GetSingleHandler(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public async Task<GetSingleResponse> Handle(GetSingleRequest request)
    {
        return new GetSingleResponse(await _mapRepository.GetSingle(request.Id));
    }
}