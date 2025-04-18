using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using System;
namespace LiveMap.Application.PointOfInterest.Handlers;

internal class UpdateSingleHandler : IRequestHandler<UpdateSingleRequest, UpdateSingleResponse>
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
