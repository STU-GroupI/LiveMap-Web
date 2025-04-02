using LiveMap.Application.RequestForChange.Persistance;

namespace LiveMap.Application.RequestForChange.Handlers;
using Application.RequestForChange.Responses;
using Application.RequestForChange.Requests;

public class CreateRequestHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{
    private readonly IRequestForChangeRepository _requestForChangeRepository;

    public CreateRequestHandler(IRequestForChangeRepository requestForChangeRepository)
    {
        _requestForChangeRepository = requestForChangeRepository;
    }
    public async Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return new CreateSingleResponse(await _requestForChangeRepository.CreateAsync(request.Rfc));
    }
}