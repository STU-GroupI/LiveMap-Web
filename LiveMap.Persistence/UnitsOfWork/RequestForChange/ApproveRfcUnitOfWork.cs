using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.UnitsOfWork;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;

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
        if (request.Poi is null)
        {
            throw new ArgumentNullException($"{nameof(request.Poi)} should not be null when approving an RFC");
        }

        PointOfInterest? poi = null;
        
        try
        {
            await using var transaction = await _liveMapContext.Database.BeginTransactionAsync();

            // remove the suggested poi from the database and create the suggested poi
            if (request.Rfc.SuggestedPoiId is Guid suggestedPoiId)
            {
                await _suggestedPointOfInterestRepository.DeleteWithoutCommitAsync(suggestedPoiId);
                request.Poi.StatusName = PointOfInterestStatus.ACTIVE;
                poi = await _pointOfInterestRepository.CreateWithoutCommitAsync(request.Poi);
                request.Rfc.SuggestedPoiId = null;
            }
            // update the required poi
            else if (request.Rfc.PoiId is not null)
            {
                poi = await _pointOfInterestRepository.UpdateWithoutCommitAsync(request.Poi);
            }

            // update rfc status with approved
            request.Rfc.ApprovalStatus = ApprovalStatus.APPROVED;
            var rfc = await _rfcRepository.UpdateWithoutCommitAsync(request.Rfc);

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
