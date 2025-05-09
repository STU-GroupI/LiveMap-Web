using LiveMap.Application.Category.Persistance;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Category.Handlers;

public class UpdateSingleHandler : IRequestHandler<UpdateSingleRequest, UpdateSingleResponse>
{

    private readonly ICategoryRepository _categoryRepository;

    public UpdateSingleHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<UpdateSingleResponse> Handle(UpdateSingleRequest request)
    {
        var success = await _categoryRepository.Update(request.oldName, request.newName, request.iconName);
        return new UpdateSingleResponse(request.oldName, request.newName, request.iconName, success);

    }
}
