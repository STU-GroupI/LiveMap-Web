namespace LiveMapDashboard.Web.Models.Dashboard;

public sealed record DashboardViewModel(
    IEnumerable<(string id, string name)> Maps,
    Guid? MapId = null,
    string? MapName = null)
{
    public static DashboardViewModel Empty => new([]);
};