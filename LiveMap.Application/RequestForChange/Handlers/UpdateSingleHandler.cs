using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;

namespace LiveMap.Application.RequestForChange.Handlers;

public class UpdateSingleHandler : IRequestHandler<UpdateSingleRequest, UpdateSingleResponse>
{
    private readonly IRequestForChangeRepository _requestForChangeRepository;

    public UpdateSingleHandler(IRequestForChangeRepository requestForChangeRepository)
    {
        _requestForChangeRepository = requestForChangeRepository;
    }

public async Task<UpdateSingleResponse> Handle(UpdateSingleRequest request)
    {
        return new UpdateSingleResponse(await _requestForChangeRepository.UpdateAsync(request.Rfc));
    }
}