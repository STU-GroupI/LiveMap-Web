namespace LiveMapDashboard.Web.Models.Category;
using Models = LiveMap.Domain.Models;

public record CategoryListViewModel(
    Models.Category[] Categories
);