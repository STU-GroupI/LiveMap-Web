using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.SuggestedPoi.Requests;

public sealed record CreateSingleRequest(SuggestedPointOfInterest SuggestedPoi);
