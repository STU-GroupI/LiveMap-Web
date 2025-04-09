namespace LiveMapDashboard.Web.Models.Poi
{
    public sealed record OpeningHoursViewModel(bool IsActive, string From, string To)
    {
        public static OpeningHoursViewModel Empty => new(true, "08:00", "17:00");
    }
}