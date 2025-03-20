using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.PointOfInterest.Responses;
public sealed record GetSingleResponse(LiveMap.Domain.Models.PointOfInterest? PointOfInterest);
