using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.PointOfInterest.Handlers;

public class GetSingleHandler
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;

    public GetSingleHandler(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
    }

    public GetSingleResponse GetFromRepo(GetSingleRequest request)
    {
        return new GetSingleResponse(_pointOfInterestRepository.GetSingle(request.Id).Result);
    }
}