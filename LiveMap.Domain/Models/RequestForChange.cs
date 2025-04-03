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

    public string ApprovalStatus => Status?.Status ?? string.Empty;
    public required ApprovalStatus Status { get; set; }
<<<<<<< HEAD
    public DateTime? ApprovedOn { get; set; }
=======
    
    public DateTime ApprovedOn { get; set; }
>>>>>>> 5cacfa20cb2c0d6944e341269a199509cfdcc956

    public DateTime SubmittedOn { get; set; }
    public required string Message { get; set; }
    public required DateTime SubmittedOn { get; set; }
}
