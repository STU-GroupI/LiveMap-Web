namespace LiveMap.Domain.Models;

public class RequestForChange
{
    public required Guid Id { get; set; }
    public Guid? PoiId { get; set; }
    public Guid? SuggestedPoiId { get; set; }

    public PointOfInterest? Poi { get; set; }
    public SuggestedPointOfInterest? SuggestedPoi { get; set; }

    public required string ApprovalStatus { get; set; } = string.Empty;
    public required DateTime SubmittedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? Message { get; set; }
}
