using LiveMap.Domain.Models;

namespace LiveMapDashboard.Web.Models.Poi
{
    public sealed record PoiListViewModel()
    {
        public List<PoiListEntryViewModel> Pois { get; init; } = new();
    }

    public sealed record PoiListEntryViewModel()
    {
        public string Id { get; init; } = Guid.Empty.ToString();
        public string Name { get; init; } = string.Empty;
    }
}