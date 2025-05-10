using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Map;

namespace LiveMapDashboard.Web.Extensions.Mappers;

public static class MapViewModelMapperExtensions
{
    public static Map ToDomainMap(this MapCrudformViewModel viewModel)
    {
        return new()
        {
            Id = viewModel.Id is not null 
                ? Guid.Parse(viewModel.Id) 
                : Guid.Empty,
            Name = viewModel.Name,
            ImageUrl = viewModel.ImageUrl,
            Bounds = new[]
            {
                viewModel.TopLeft,
                viewModel.TopRight,
                viewModel.BottomLeft,
                viewModel.BottomRight
            }, 
            Area = new[]
            {
                viewModel.TopLeft,
                viewModel.TopRight,
                viewModel.BottomLeft,
                viewModel.BottomRight
            }
        };
    }
}