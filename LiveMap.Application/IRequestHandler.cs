namespace LiveMap.Application;
public interface IRequestHandler<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request);
}

public interface IRequestHandler<TRequest>
{
    public Task<bool> Handle(TRequest request);
}
