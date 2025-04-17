using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Suggestions;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class PoiSuggestionsViewModelProvider : IViewModelProvider<PoiSuggestionsViewModel>
    {
        private readonly ISuggestedPointOfInterestService _suggestedPoiService;

        public PoiSuggestionsViewModelProvider(ISuggestedPointOfInterestService suggestedPoiService)
        {
            _suggestedPoiService = suggestedPoiService;
        }

        public async Task<PoiSuggestionsViewModel> Hydrate(PoiSuggestionsViewModel viewModel)
        {
            SuggestedPointOfInterest[] suggestions = (await _suggestedPoiService.Get(viewModel.MapId, null, null, null)).Value ?? [];

            return viewModel with
            {
                MapId = viewModel.MapId,
                SuggestedPointOfInterests = suggestions
            };
        }

        public async Task<PoiSuggestionsViewModel> Provide()
        {
            return await Hydrate(PoiSuggestionsViewModel.Empty);
        }
    }
}