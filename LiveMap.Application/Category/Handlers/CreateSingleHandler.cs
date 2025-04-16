using LiveMap.Application.Category.Persistance;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Category.Handlers;

public class CreateSingleHandler : IRequestHandler<CreateSingleRequest, CreateSingleResponse>
{

    private readonly ICategoryRepository _categoryRepository;

    public CreateSingleHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateSingleResponse> Handle(CreateSingleRequest request)
    {
        return new(await _categoryRepository.Create(request.Category));
    }
}
