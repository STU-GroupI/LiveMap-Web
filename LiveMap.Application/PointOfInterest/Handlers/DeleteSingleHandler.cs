﻿using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;

namespace LiveMap.Application.PointOfInterest.Handlers;

public class DeleteSingleHandler : IRequestHandler<DeleteSingleRequest, DeleteSingleResponse>
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;
    public DeleteSingleHandler(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
    }
    public async Task<DeleteSingleResponse> Handle(DeleteSingleRequest request)
    {
        return new DeleteSingleResponse(await _pointOfInterestRepository.DeleteSingle(request.Id));
    }
}
