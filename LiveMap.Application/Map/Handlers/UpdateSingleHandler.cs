using LiveMap.Application.Map.Persistance;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;

namespace LiveMap.Application.Map.Handlers;
public class UpdateSingleHandler : IRequestHandler<UpdateSingleRequest, UpdateSingleResponse>
{

    private readonly IMapRepository _mapRepository;

    public UpdateSingleHandler(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public async Task<UpdateSingleResponse> Handle(UpdateSingleRequest request)
    {
        return new(await _mapRepository.Update(request.Map));
    }
}