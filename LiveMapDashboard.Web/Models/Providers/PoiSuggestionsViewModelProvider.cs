using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Rfc;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class PoiSuggestionsViewModelProvider : IViewModelProvider<RFCsViewModel>
    {
        private readonly IRequestForChangeService _suggestedPoiService;

        public PoiSuggestionsViewModelProvider(IRequestForChangeService suggestedPoiService)
        {
            _suggestedPoiService = suggestedPoiService;
        }

        public async Task<RFCsViewModel> Hydrate(RFCsViewModel viewModel)
        {
            RequestForChange[] suggestions = (await _suggestedPoiService.Get(viewModel.MapId, viewModel.Skip, viewModel.Take, viewModel.Ascending)).Value ?? [];

            return viewModel with
            {
                MapId = viewModel.MapId,
                RFCs = suggestions
            };
        }

        public async Task<RFCsViewModel> Provide()
        {
            return await Hydrate(RFCsViewModel.Empty);
        }
    }
}