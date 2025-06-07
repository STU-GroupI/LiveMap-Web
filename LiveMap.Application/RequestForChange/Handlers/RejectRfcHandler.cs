using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Application.RequestForChange.UnitsOfWork;

namespace LiveMap.Application.RequestForChange.Handlers;

public class RejectRfcHandler : IRequestHandler<RejectRfcRequest, RejectRfcResponse>
{
    private readonly IRejectRfcUnitOfWork _unitOfWork;

    public RejectRfcHandler(IRejectRfcUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RejectRfcResponse> Handle(RejectRfcRequest request)
    {
        return new(await _unitOfWork.CommitAsync(request));
    }
}
