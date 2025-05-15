using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Models.Category;

namespace LiveMapDashboard.Web.Models.Providers;
public class CategoryCrudFormViewModelProvider : IViewModelProvider<CategoryCrudFormViewModel>
{
    private readonly ICategoryService _categoryService;
    
    public CategoryCrudFormViewModelProvider(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    public async Task<CategoryCrudFormViewModel> Hydrate(CategoryCrudFormViewModel viewModel)
    {
        if (string.IsNullOrEmpty(viewModel.CategoryName))
        {
            return viewModel;
        }
        
        var category = (await _categoryService.Get(viewModel.CategoryName)).Value;

        return viewModel with
        {
            IsUsed = category?.InUse
        };
    }

    public async Task<CategoryCrudFormViewModel> Provide()
    {
        return await Hydrate(new(string.Empty, string.Empty, string.Empty, false));
    }
}
