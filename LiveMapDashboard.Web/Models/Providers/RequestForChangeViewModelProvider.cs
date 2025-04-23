using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Rfc;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class RequestForChangeViewModelProvider : IViewModelProvider<RequestForChangeViewModel>
    {
        private readonly IRequestForChangeService _rfcService;

        public RequestForChangeViewModelProvider(IRequestForChangeService rfcService)
        {
            _rfcService = rfcService;
        }

        public async Task<RequestForChangeViewModel> Hydrate(RequestForChangeViewModel viewModel)
        {
            PaginatedResult<RequestForChange> result = (await _rfcService.Get(viewModel.MapId, viewModel.Skip, viewModel.Take, viewModel.Ascending)).Value ?? PaginatedResult<RequestForChange>.Default;

            return viewModel with
            {
                MapId = viewModel.MapId,
                Result = result
            };
        }

        public async Task<RequestForChangeViewModel> Provide()
        {
            return await Hydrate(RequestForChangeViewModel.Empty);
        }
    }
}