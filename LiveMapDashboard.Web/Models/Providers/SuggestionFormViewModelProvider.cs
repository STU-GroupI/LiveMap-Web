using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Suggestion;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class SuggestionFormViewModelProvider : IViewModelProvider<SuggestionFormViewModel>
    {
        private readonly ISuggestedPoIService _suggestedPoiService;
        private readonly ICategoryService _categoryService;
        private readonly IMapService _mapService;
        private readonly IRequestForChangeService _rfcService;

        public SuggestionFormViewModelProvider(
            ISuggestedPoIService suggestedPoiService,
            ICategoryService categoryService,
            IMapService mapService,
            IRequestForChangeService rfcService)
        {
            _suggestedPoiService = suggestedPoiService;
            _categoryService = categoryService;
            _mapService = mapService;
            _rfcService = rfcService;

        }

        public async Task<SuggestionFormViewModel> Hydrate(SuggestionFormViewModel viewModel)
        {

            // Get RFC data first
            var rfcResponse = await _rfcService.Get(Guid.Parse(viewModel.RfcId));

            if (!rfcResponse.IsSuccess || rfcResponse.Value == null)
            {
                return viewModel with { RfcId = "No RFC was found" };
            }

            BackendApiHttpResponse<SuggestedPointOfInterest> suggestedPoiResult =
                await _suggestedPoiService.Get(Guid.Parse(viewModel.RfcId));
            BackendApiHttpResponse<Category[]> categoriesResult = await _categoryService.Get(null, null);
            BackendApiHttpResponse<Map[]> mapsResult = await _mapService.Get(null, null);

            SuggestedPointOfInterest suggestedPoi = suggestedPoiResult.Value ?? default!;

            return viewModel with
            {
                RfcId = suggestedPoi?.RFCId.ToString() ?? "No RFC was found",
                Message = suggestedPoi?.RFC?.Message ?? "No message was found",
                Categories = categoriesResult.Value ?? [],
                MapId = mapsResult.Value?[0].Id.ToString() ?? "No map was given"
            };
        }

        public async Task<SuggestionFormViewModel> Provide()
        {
            return await Hydrate(SuggestionFormViewModel.EmptyWithId(string.Empty));
        }
    }
}
