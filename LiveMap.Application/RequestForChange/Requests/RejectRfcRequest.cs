using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.RequestForChange.Requests;

public sealed record RejectRfcRequest(Guid RfcId);
