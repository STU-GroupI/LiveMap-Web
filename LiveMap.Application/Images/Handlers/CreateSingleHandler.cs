using LiveMap.Application.Images.Requests;
using LiveMap.Application.Images.Responses;

namespace LiveMap.Application.Images.Handlers;

public class CreateSingleHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{
    public Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return Task.FromResult(new CreateSingleResponse(request.ImageUrl));
    }
}
