﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.SuggestedPoi.Requests;

public sealed record GetMultipleRequest(Guid Id, int? Skip, int? Take);
