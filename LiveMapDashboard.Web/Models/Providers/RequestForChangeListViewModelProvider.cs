using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Rfc;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class RequestForChangeListViewModelProvider : IViewModelProvider<RequestForChangeListViewModel>
    {
        private readonly IRequestForChangeService _rfcService;

        public RequestForChangeListViewModelProvider(IRequestForChangeService rfcService)
        {
            _rfcService = rfcService;
        }

        public async Task<RequestForChangeListViewModel> Hydrate(RequestForChangeListViewModel viewModel)
        {
            PaginatedResult<RequestForChange> result = (await _rfcService.GetMultiple(viewModel.MapId, viewModel.Skip, viewModel.Take, viewModel.Ascending)).Value ?? PaginatedResult<RequestForChange>.Default;

            return viewModel with
            {
                MapId = viewModel.MapId,
                Result = result
            };
        }

        public async Task<RequestForChangeListViewModel> Provide()
        {
            return await Hydrate(RequestForChangeListViewModel.Empty);
        }
    }
}