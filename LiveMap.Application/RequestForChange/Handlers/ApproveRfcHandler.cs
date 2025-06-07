using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Application.RequestForChange.UnitsOfWork;

namespace LiveMap.Application.RequestForChange.Handlers;

public class ApproveRfcHandler : IRequestHandler<ApprovalRequest, ApprovalResponse>
{
    private readonly IApproveRfcUnitOfWork _unitOfWork;

    public ApproveRfcHandler(IApproveRfcUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApprovalResponse> Handle(ApprovalRequest request)
    {
        return new(await _unitOfWork.CommitAsync(request));
    }
}
