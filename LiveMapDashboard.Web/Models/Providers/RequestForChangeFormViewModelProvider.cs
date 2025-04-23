using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Rfc;

namespace LiveMapDashboard.Web.Models.Providers;

public class RequestForChangeFormViewModelProvider : IViewModelProvider<RequestForChangeFormViewModel>
{
    private readonly IViewModelProvider<PoiCrudformViewModel> _crudViewModelProvider;
    private readonly IRequestForChangeService _requestForChangeService;
    private readonly IPointOfInterestService _pointOfInterestService;
    private readonly ISuggestedPointOfInterestService _suggestedPointOfInterestService;

    public RequestForChangeFormViewModelProvider(
        IViewModelProvider<PoiCrudformViewModel> crudViewModelProvider,
        IRequestForChangeService requestForChangeService,
        IPointOfInterestService pointOfInterestService,
        ISuggestedPointOfInterestService suggestedPointOfInterestService)
    {
        _crudViewModelProvider = crudViewModelProvider;
        _requestForChangeService = requestForChangeService;
        _pointOfInterestService = pointOfInterestService;
        _suggestedPointOfInterestService = suggestedPointOfInterestService;
    }

    public async Task<RequestForChangeFormViewModel> Hydrate(RequestForChangeFormViewModel viewModel)
    {
        // get RFC
        BackendApiHttpResponse<RequestForChange> rfcResponse = await _requestForChangeService.Get(viewModel.Rfc.Id);
        RequestForChange rfc = rfcResponse.Value
            ?? throw new Exception($"RFC not found during hydration from {this.GetType().FullName} for {viewModel.ToString()}");
        // if RFC has POI, get THAT one
        BackendApiHttpResponse<PointOfInterest>? poiResult = rfc.PoiId switch
        {
            var poiId when poiId is not null => await _pointOfInterestService.Get(rfc.PoiId!.Value),
            _ => null
        };

        // if RFC has suggested POI, get THAT one
        BackendApiHttpResponse<SuggestedPointOfInterest>? suggestedPoiResult = rfc.SuggestedPoiId switch
        {
            var poiId when poiId is not null => await _suggestedPointOfInterestService.Get(rfc.SuggestedPoiId!.Value),
            _ => null
        };
        SuggestedPointOfInterest? suggestedPoi = suggestedPoiResult?.Value ?? null;

        var crudFormViewModel = poiResult switch
        {
            { IsSuccess: true, Value: PointOfInterest poi } => await _crudViewModelProvider.Provide()
                with
                {
                    Id = poi.Id.ToString(),
                    MapId = poi.MapId.ToString(),
                    Title = poi.Title,
                    Description = poi.Description,
                    Category = poi.CategoryName,
                    Coordinate = poi.Coordinate,
                    IsWheelchairAccessible = poi.IsWheelchairAccessible,
                    OpeningHours = poi.OpeningHours.Select(oh => oh.ToViewModelOpeningHours()).ToArray(),
                },
            _ when suggestedPoi is not null => await _crudViewModelProvider.Provide()
                with
                {
                    Id = suggestedPoi.Id.ToString(),
                    MapId = suggestedPoi.MapId.ToString(),
                    Title = suggestedPoi.Title,
                    Description = suggestedPoi.Description,
                    Category = suggestedPoi.CategoryName,
                    Coordinate = suggestedPoi.Coordinate,
                    IsWheelchairAccessible = suggestedPoi.IsWheelchairAccessible,
                }
        };

        return new RequestForChangeFormViewModel(rfc, crudFormViewModel);
    }

    public Task<RequestForChangeFormViewModel> Provide()
    {
        throw new NotImplementedException();
    }
}