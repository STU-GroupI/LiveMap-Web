using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.UnitsOfWork;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Persistence.UnitsOfWork.RequestForChange;

public class RejectRfcUnitOfWork : IRejectRfcUnitOfWork
{
    private readonly RequestForChangeRepository _rfcRepository;
    private readonly SuggestedPointOfInterestRepository _suggestedPointOfInterestRepository;
    private readonly LiveMapContext _liveMapContext;

    public RejectRfcUnitOfWork(LiveMapContext liveMapContext)
    {
        _liveMapContext = liveMapContext;
        _rfcRepository = new(liveMapContext);
        _suggestedPointOfInterestRepository = new(liveMapContext);
    }

    public async Task<bool> CommitAsync(RejectRfcRequest request)
    {
        DomainModels.RequestForChange? rfc = await _rfcRepository.GetSingle(request.RfcId);
        
        if (rfc is null)
        {
            return false;
        }

        rfc.ApprovalStatus = ApprovalStatus.REJECTED;
        
        try
        {
            await using var transaction = await _liveMapContext.Database.BeginTransactionAsync();

            if(rfc.SuggestedPoiId is not null)
            {
                await _suggestedPointOfInterestRepository.DeleteWithoutCommitAsync(rfc.SuggestedPoiId.Value);
                rfc.SuggestedPoiId = null;
            }
            await _rfcRepository.UpdateWithoutCommitAsync(rfc);

            await _liveMapContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
