using LiveMap.Application.Map.Persistance;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;
using LiveMap.Application.PointOfInterest.Persistance;

namespace LiveMap.Application.Map.Handlers;
public class DeleteSingleHandler : IRequestHandler<DeleteSingleRequest>
{

    private readonly IMapRepository _mapRepository;

    public DeleteSingleHandler(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public async Task<bool> Handle(DeleteSingleRequest request)
    {
        bool response = await _mapRepository.Delete(request.Id);
        return response;
    }
}