using LiveMap.Application.SuggestedPoi.Persistanc;
using LiveMap.Application.SuggestedPoi.Requests;
using LiveMap.Application.SuggestedPoi.Responses;

namespace LiveMap.Application.SuggestedPoi.Handlers;

public class GetMultipleHandler : IRequestHandler<GetMultipleRequest, GetMultipleResponse>
{
    private readonly ISuggestedPointOfInterestRepository _suggestedPointOfInterestRepository;

    public GetMultipleHandler(ISuggestedPointOfInterestRepository suggestedPointOfInterestRepository)
    {
        _suggestedPointOfInterestRepository = suggestedPointOfInterestRepository;
    }

    public async Task<GetMultipleResponse> Handle(GetMultipleRequest request)
    {
        return new GetMultipleResponse(
            await _suggestedPointOfInterestRepository.GetMultiple(
                request.Id, request.Skip, request.Take, request.Ascending
                ));
    }
}
