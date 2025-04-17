namespace LiveMapDashboard.Web.Models.Poi
{
    public sealed record PoiListViewModel()
    {
        public List<Poi> Pois { get; init; } = new();
    }
}