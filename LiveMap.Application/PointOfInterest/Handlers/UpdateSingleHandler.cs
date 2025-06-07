using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
namespace LiveMap.Application.PointOfInterest.Handlers;

public class UpdateSingleHandler : IRequestHandler<UpdateSingleRequest, UpdateSingleResponse>
{
    private readonly IPointOfInterestRepository _repository;

    public UpdateSingleHandler(IPointOfInterestRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateSingleResponse> Handle(UpdateSingleRequest request)
    {
        return new(await _repository.Update(request.Poi));
    }
}
