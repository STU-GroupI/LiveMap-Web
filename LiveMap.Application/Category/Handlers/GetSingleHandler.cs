using LiveMap.Application.Category.Persistance;
using LiveMap.Application.Category.Requests;
using LiveMap.Application.Category.Responses;

namespace LiveMap.Application.Category.Handlers
{
    public class GetSingleHandler : IRequestHandler<GetSingleRequest, GetSingleResponse>
    {
        private readonly ICategoryRepository _repository;

        public GetSingleHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSingleResponse> Handle(GetSingleRequest request)
        {
            return new(await _repository.GetSingle(request.Name));
        }
    }
}
