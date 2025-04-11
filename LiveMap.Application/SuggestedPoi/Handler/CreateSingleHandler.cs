using LiveMap.Application.SuggestedPoi.Persistanc;
using LiveMap.Application.SuggestedPoi.Requests;
using LiveMap.Application.SuggestedPoi.Responses;

namespace LiveMap.Application.SuggestedPoi.Handlers;

public class CreateSingleHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{

    private readonly ISuggestedPointOfInterestRepository _repo;

    public CreateSingleHandler(ISuggestedPointOfInterestRepository repo)
    {
        _repo = repo;
    }

    public async Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return new(await _repo.CreateSuggestedPointOfInterest(request.SuggestedPoi));
    }
}
