using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;

namespace LiveMap.Application.RequestForChange.Handlers;

public class GetSingleHandler : IRequestHandler<GetSingleRequest, GetSingleResponse>
{
    private readonly IRequestForChangeRepository _requestForChangeRepository;

    public GetSingleHandler(IRequestForChangeRepository requestForChangeRepository)
    {
        _requestForChangeRepository = requestForChangeRepository;
    }

    public async Task<GetSingleResponse> Handle(GetSingleRequest request)
    {
        return new GetSingleResponse(await _requestForChangeRepository.GetSingle(request.Id));
    }
}