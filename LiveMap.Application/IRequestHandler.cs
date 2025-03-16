using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application;
public interface IRequestHandler<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request);
}
