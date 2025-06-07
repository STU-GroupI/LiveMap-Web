using LiveMap.Application.Category.Persistance;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;

namespace LiveMap.Application.Category.Handlers;

public class DeleteSingleHandler : IRequestHandler<DeleteSingleRequest, DeleteSingleResponse>
{

    private readonly ICategoryRepository _categoryRepository;

    public DeleteSingleHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<DeleteSingleResponse> Handle(DeleteSingleRequest request)
    {
        return new(await _categoryRepository.Delete(request.name));
    }
}
