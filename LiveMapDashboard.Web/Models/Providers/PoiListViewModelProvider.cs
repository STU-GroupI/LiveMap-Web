using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class PoiListViewModelProvider : IViewModelProvider<PoiListViewModel>
    {
        public Task<PoiListViewModel> Provide()
        {
            var viewModel = new PoiListViewModel
            {
                Pois = new List<Poi.Poi>
                {
                    new Poi.Poi
                    {
                        Id = 1,
                        Name = "POI 1",
                        Description = "Description for POI 1",
                        Latitude = 40.7128,
                        Longitude = -74.0060
                    },
                    new Poi.Poi
                    {
                        Id = 2,
                        Name = "POI 2",
                        Description = "Description for POI 2",
                        Latitude = 34.0522,
                        Longitude = -118.2437
                    },
                    new Poi.Poi
                    {
                        Id = 3,
                        Name = "POI 3",
                        Description = "Description for POI 3",
                        Latitude = 51.5074,
                        Longitude = -0.1278
                    },
                }
            };

            return Task.FromResult(viewModel);
        }

        public Task<PoiListViewModel> Hydrate(PoiListViewModel viewModel)
        {
            return Task.FromResult(viewModel);
        }
    }
}