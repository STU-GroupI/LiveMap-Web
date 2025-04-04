using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Persistence.DbModels;
public class SqlRequestForChange
{
    public Guid Id { get; set; }

    public Guid? PoiId { get; set; }
    public SqlPointOfInterest? Poi { get; set; }

    public Guid? SuggestedPoiId { get; set; }
    public SqlSuggestedPointOfInterest? SuggestedPoi { get; set; }
    
    public required string ApprovalStatus { get; set; }

    public string? Message { get; set; }
    public required DateTime SubmittedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
}