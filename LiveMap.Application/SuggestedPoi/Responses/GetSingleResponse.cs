﻿using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.SuggestedPoi.Responses;

public sealed record GetSingleResponse(SuggestedPointOfInterest? SuggestedPoi);
