
namespace LiveMap.Domain.Models;

public class RequestForChange
{
    public Guid Id { get; set; }
    public Guid? PoiId { get; set; }
    public Guid? SuggestedPoiId { get; set; }

    public PointOfInterest? Poi { get; set; }
    public SuggestedPointOfInterest? SuggestedPoi { get; set; }

    public string ApprovalStatus { get; set; } = string.Empty;

    public string? Message { get; set; }
}
