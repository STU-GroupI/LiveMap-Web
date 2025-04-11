using LiveMap.Application.Map.Persistance;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;

namespace LiveMap.Application.Map.Handlers;

public class GetMultipleHandler : IRequestHandler<GetMultipleRequest, GetMultipleResponse>
{
    private readonly IMapRepository _mapRepository;

    public GetMultipleHandler(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public async Task<GetMultipleResponse> Handle(GetMultipleRequest request)
    {
        return new GetMultipleResponse(
            await _mapRepository.GetMultiple(
                request.Skip,
                request.Take));
    }
}