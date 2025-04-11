using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;

namespace LiveMap.Application.PointOfInterest.Handlers;

public class GetMultipleHandler : IRequestHandler<GetMultipleRequest, GetMultipleResponse>
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;

    public GetMultipleHandler(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
    }

    public async Task<GetMultipleResponse> Handle(GetMultipleRequest request)
    {
        return new GetMultipleResponse(
            await _pointOfInterestRepository.GetMultiple(
                request.MapId,
                request.Skip,
                request.Take));
    }
}