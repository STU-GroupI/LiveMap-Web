using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.RequestForChange.Handlers;
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
