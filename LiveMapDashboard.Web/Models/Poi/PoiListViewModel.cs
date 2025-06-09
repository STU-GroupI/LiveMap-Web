namespace LiveMapDashboard.Web.Models.Poi
{
    public sealed record PoiListViewModel()
    {
        public Guid MapId { get; set; }
        public List<PoiListEntryViewModel> Pois { get; init; } = new();
    }

    public sealed record PoiListEntryViewModel()
    {
        public string Id { get; init; } = Guid.Empty.ToString();
        public string Name { get; init; } = string.Empty;
    }
}