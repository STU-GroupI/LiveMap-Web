using LiveMap.Application.Map.Persistance;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;

namespace LiveMap.Application.Map.Handlers;
public class CreateSingleHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{

    private readonly IMapRepository _mapRepository;

    public CreateSingleHandler(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public async Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return new(await _mapRepository.CreateAsync(request.Map));
    }
}
