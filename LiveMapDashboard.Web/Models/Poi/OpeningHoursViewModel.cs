namespace LiveMapDashboard.Web.Models.Poi
{
    public sealed record OpeningHoursViewModel(bool IsActive, string From, string To)
    {
        public static OpeningHoursViewModel Empty => new(false, "00:00", "00:00");
    }
}