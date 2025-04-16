using LiveMap.Application.Category.Persistance;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Category.Handlers;

public class GetSingleHandler : IRequestHandler<GetSingleRequest, GetSingleResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetSingleHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetSingleResponse> Handle(GetSingleRequest request)
    {
        return new GetSingleResponse(await _categoryRepository.GetSingle(request.name));
    }
}