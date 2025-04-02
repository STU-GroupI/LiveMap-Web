using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Models;

public class RequestForChange
{
    public Guid Id { get; set; }
    public Guid? PoiId { get; set; }
    public Guid? SuggestedPoiId { get; set; }

    public PointOfInterest? Poi { get; set; }
    public SuggestedPointOfInterest? SuggestedPoi { get; set; }

    public string ApprovalStatus => Status.Status;
    public required ApprovalStatus Status { get; set; }

    public required string Message { get; set; }
}
