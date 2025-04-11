using LiveMap.Application.RequestForChange.Persistance;

namespace LiveMap.Application.RequestForChange.Handlers;
using Application.RequestForChange.Requests;
using Application.RequestForChange.Responses;

public class CreateSingleHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{
    private readonly IRequestForChangeRepository _requestForChangeRepository;

    public CreateSingleHandler(IRequestForChangeRepository requestForChangeRepository)
    {
        _requestForChangeRepository = requestForChangeRepository;
    }
    public async Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return new CreateSingleResponse(await _requestForChangeRepository.CreateAsync(request.Rfc));
    }
}