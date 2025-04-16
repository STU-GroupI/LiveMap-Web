namespace LiveMapDashboard.Web.Models.Poi
{
    public sealed record OpeningHoursViewModel(bool IsActive, string Start, string End)
    {
        public static OpeningHoursViewModel Empty => new(true, "08:00", "17:00");
    }
}