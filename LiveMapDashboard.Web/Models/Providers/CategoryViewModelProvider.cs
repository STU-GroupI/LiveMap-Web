using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Models.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveMapDashboard.Web.Models.Providers;

/// <summary>
/// Provides <see cref="CategoryViewModel"/> instances by fetching domain categories via <see cref="ICategoryService"/>.
/// </summary>
public class CategoryViewModelProvider : IViewModelProvider<IEnumerable<CategoryViewModel>>
{
    private readonly ICategoryService _categoryService;

    public CategoryViewModelProvider(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Hydrates the incoming view model placeholder by loading all categories from the domain service.
    /// </summary>
    /// <param name="placeholder">An optional placeholder used to pass parameters (unused).</param>
    /// <returns>A sequence of <see cref="CategoryViewModel"/> representing all available categories.</returns>
    public async Task<IEnumerable<CategoryViewModel>> Hydrate(IEnumerable<CategoryViewModel> placeholder)
    {
        // Fetch all domain categories
        var domainCategories = await _categoryService.GetAllAsync();

        // Map to view models
        var viewModels = domainCategories
            .Select(c => new CategoryViewModel(c.CategoryName))
            .ToList();

        return viewModels;
    }

    /// <summary>
    /// Provides a default list of categories by hydrating an empty placeholder.
    /// </summary>
    public Task<IEnumerable<CategoryViewModel>> Provide()
        => Hydrate(Enumerable.Empty<CategoryViewModel>());
}
