using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Categories;
using LiveMapDashboard.Web.Models.Suggestions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LiveMapDashboard.Web.Models.Providers;

public class CategoryViewModelProvider : IViewModelProvider<CategoryViewModel>
{
    private readonly ICategoryService _categoryService;

    public CategoryViewModelProvider(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<CategoryViewModel> Hydrate(CategoryViewModel viewModel)
    {
        Category[] categories = (await _categoryService.Get(viewModel.Skip, viewModel.Take)).Value ?? [];

        return viewModel with
        {
            Categories = categories
        };
    }

    public async Task<CategoryViewModel> Provide()
    {
        return await Hydrate(CategoryViewModel.Empty);
    }
}