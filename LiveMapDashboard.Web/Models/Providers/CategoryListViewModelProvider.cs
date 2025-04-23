using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Models.Category;

namespace LiveMapDashboard.Web.Models.Providers;
using Models = LiveMap.Domain.Models;

public class CategoryListViewModelProvider : IViewModelProvider<CategoryListViewModel>
{
    private readonly ICategoryService _categoryService;

    public CategoryListViewModelProvider(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<CategoryListViewModel> Hydrate(CategoryListViewModel viewModel)
    {
        var categoriesResult = await _categoryService.Get(null, null);

        Models.Category[] categories = categoriesResult switch
        {
            { IsSuccess: true, Value: Models.Category[] value } => value,
            _ => []
        };

        return viewModel with
        {
            Categories = categories
        };
    }

    public async Task<CategoryListViewModel> Provide()
    {
        return await Hydrate(new CategoryListViewModel([]));
    }
}