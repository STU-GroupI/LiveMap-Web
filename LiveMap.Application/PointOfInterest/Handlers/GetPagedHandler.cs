using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.PointOfInterest.Handlers;

public class GetPagedHandler : IRequestHandler<GetPagedRequest, GetPagedResponse>
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;

    public GetPagedHandler(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
    }

    public async Task<GetPagedResponse> Handle(GetPagedRequest request)
    {
        var data = await _pointOfInterestRepository.GetPaged(1, 2, 3);
        return new GetPagedResponse();
    }
}