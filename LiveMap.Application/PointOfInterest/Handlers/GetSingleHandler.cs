﻿using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;

namespace LiveMap.Application.PointOfInterest.Handlers;

public class GetSingleHandler : IRequestHandler<GetSingleRequest, GetSingleResponse>
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;

    public GetSingleHandler(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
    }

    public async Task<GetSingleResponse> Handle(GetSingleRequest request)
    {
        return new GetSingleResponse(await _pointOfInterestRepository.GetSingle(request.Id));
    }
}