﻿using System;
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

    public string ApprovalStatus { get; set; } = string.Empty;
    public required DateTime SubmittedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? Message { get; set; }
}
