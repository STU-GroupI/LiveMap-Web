namespace LiveMap.Application;
public interface IRequestHandler<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request);
}
