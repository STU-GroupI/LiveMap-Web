using Model = LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using System.ComponentModel.DataAnnotations;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Map;

public sealed record MapListViewModel()
{
    public List<MapListEntryViewModel> Maps { get; init; } = new();
}

public sealed record MapListEntryViewModel()
{
    public string Id { get; init; } = Guid.Empty.ToString();
    public string Name { get; init; } = string.Empty;
}