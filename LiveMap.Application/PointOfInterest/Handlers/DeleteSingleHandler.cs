using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;

namespace LiveMap.Application.PointOfInterest.Handlers;

public class DeleteSingleHandler : IRequestHandler<DeleteSingleRequest>
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;
    public DeleteSingleHandler(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
    }
    public async Task<bool> Handle(DeleteSingleRequest request)
    {
        return await _pointOfInterestRepository.DeleteSingle(request.Id);
    }
}
