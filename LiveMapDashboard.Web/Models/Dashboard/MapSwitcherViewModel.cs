namespace LiveMapDashboard.Web.Models.Dashboard;

public sealed record MapSwitcherViewModel(
    IEnumerable<(string id, string name)> Maps,
    Guid? MapId = null,
    string? MapName = null)
{
    public static MapSwitcherViewModel Empty => new([]);
};