using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.UnitsOfWork;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using System.Transactions;

namespace LiveMap.Persistence.UnitsOfWork.RequestForChange;

public class ApproveRfcUnitOfWork : IApproveRfcUnitOfWork
{
    private readonly RequestForChangeRepository _rfcRepository;
    private readonly PointOfInterestRepository _pointOfInterestRepository;
    private readonly SuggestedPointOfInterestRepository _suggestedPointOfInterestRepository;
    private readonly LiveMapContext _liveMapContext;

    public ApproveRfcUnitOfWork(
        LiveMapContext liveMapContext)
    {
        _rfcRepository = new (liveMapContext);
        _pointOfInterestRepository = new (liveMapContext);
        _suggestedPointOfInterestRepository = new(liveMapContext);
        _liveMapContext = liveMapContext;
    }

    public async Task<bool> CommitAsync(ApprovalRequest request)
    {
        try
        {
            PointOfInterest? poi = null;
            if(request.Poi is null)
            {
                throw new ArgumentNullException($"{nameof(request.Poi)} should not be null when approving an RFC");
            }

            await using var transaction = await _liveMapContext.Database.BeginTransactionAsync();

            // update rfc status with approved
            var rfc = await _rfcRepository.UpdateWithoutCommitAsync(request.Rfc);
        
            // remove the suggested poi from the database and create the suggested poi
            if(request.Rfc.SuggestedPoiId is Guid suggestedPoiId)
            {
                await _suggestedPointOfInterestRepository.DeleteWithoutCommitAsync(suggestedPoiId);
                poi = await _pointOfInterestRepository.CreateWithoutCommitAsync(request.Poi);
            }
            // update the required poi
            else if(request.Rfc.PoiId is not null)
            {
                poi = await _pointOfInterestRepository.UpdateWithoutCommitAsync(request.Poi);
            }

            await _liveMapContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (TransactionAbortedException)
        {
            return false;
        }
    }
}
