using LiveMap.Application.Persistance;
using LiveMap.Application.RequestForChange.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.RequestForChange.UnitsOfWork;

public interface IRejectRfcUnitOfWork : IUnitOfWork<RejectRfcRequest, bool>
{
}