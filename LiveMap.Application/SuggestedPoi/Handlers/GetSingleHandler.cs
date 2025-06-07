using LiveMap.Application.SuggestedPoi.Persistanc;
using LiveMap.Application.SuggestedPoi.Requests;
using LiveMap.Application.SuggestedPoi.Responses;

namespace LiveMap.Application.SuggestedPoi.Handlers;

public class GetSingleHandler : IRequestHandler<GetSingleRequest, GetSingleResponse>
{
    private readonly ISuggestedPointOfInterestRepository _suggestedPointOfInterestRepository;

    public GetSingleHandler(ISuggestedPointOfInterestRepository suggestedPointOfInterestRepository)
    {
        _suggestedPointOfInterestRepository = suggestedPointOfInterestRepository;
    }

    public async Task<GetSingleResponse> Handle(GetSingleRequest request)
    {
        return new(await _suggestedPointOfInterestRepository.ReadSingle(request.Id));
    }
}
