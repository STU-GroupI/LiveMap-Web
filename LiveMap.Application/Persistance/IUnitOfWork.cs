using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.Persistance;

public interface IUnitOfWork<TParam, TResponse>
{
    public Task<TResponse> CommitAsync(TParam param);
}

public interface IUnitOfWork<TParam>
{
    public Task CommitAsync(TParam param);
}

public interface IUnitOfWork
{
    public Task CommitAsync();
}
