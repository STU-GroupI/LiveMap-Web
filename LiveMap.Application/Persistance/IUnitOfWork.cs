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
