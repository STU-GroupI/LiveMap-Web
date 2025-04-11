using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;

namespace LiveMap.Application.PointOfInterest.Handlers;
public class CreateSingleHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{

    private readonly IPointOfInterestRepository _repo;

    public CreateSingleHandler(IPointOfInterestRepository repo)
    {
        _repo = repo;
    }

    public async Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return new(await _repo.CreatePointOfInterest(request.Poi));
    }
}
