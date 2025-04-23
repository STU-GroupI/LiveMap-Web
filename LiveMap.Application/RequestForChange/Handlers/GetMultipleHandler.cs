using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;

namespace LiveMap.Application.RequestForChange.Handlers;

public class GetMultipleHandler : IRequestHandler<GetMultipleRequest, GetMultipleResponse>
{
    private readonly IRequestForChangeRepository _requestForChangeRepository;

    public GetMultipleHandler(IRequestForChangeRepository requestForChangeRepository)
    {
        _requestForChangeRepository = requestForChangeRepository;
    }

    public async Task<GetMultipleResponse> Handle(GetMultipleRequest request)
    {
        return new GetMultipleResponse(
            await _requestForChangeRepository.GetMultiple(
                request.MapId, request.Skip, request.Take, request.Ascending
                ));
    }
}
