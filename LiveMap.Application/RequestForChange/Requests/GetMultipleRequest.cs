using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.RequestForChange.Requests;

public sealed record GetMultipleRequest(Guid MapId, int? Skip, int? Take, bool? Ascending, bool? IsPending);
