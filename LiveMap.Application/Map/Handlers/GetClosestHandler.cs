using LiveMap.Application.Map.Persistance;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;

namespace LiveMap.Application.Map.Handlers;

public class GetClosestHandler : IRequestHandler<GetClosestRequest, GetClosestResponse>
{
    private readonly IMapRepository _mapRepository;

    public GetClosestHandler(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public async Task<GetClosestResponse> Handle(GetClosestRequest request)
    {
        return new GetClosestResponse(await _mapRepository.GetClosest(request.latitude, request.longitude));
    }
}