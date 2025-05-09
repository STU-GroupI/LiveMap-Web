using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Extensions.Controllers;
using LiveMapDashboard.Web.Models.Category;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers;
using Models = LiveMap.Domain.Models;

[Route("category")]
public class CategoryController : Controller
{
    public async Task<IActionResult> Index(
        [FromServices] IViewModelProvider<CategoryListViewModel> provider)
    {
        return View(await provider.Provide());
    }

    [Route("form")]
    public async Task<IActionResult> CreateForm(
        [FromServices] IViewModelProvider<CategoryCrudFormViewModel> provider)
    {
        return View("CategoryForm", await provider.Hydrate(new(string.Empty, string.Empty, string.Empty, false)));
    }

    [Route("form/{name}")]
    public async Task<IActionResult> EditForm(
        [FromRoute] string? name,
        [FromServices] IViewModelProvider<CategoryCrudFormViewModel> provider)
    {
        var categoryService = HttpContext.RequestServices.GetService<ICategoryService>();
        var category = await categoryService?.Get(name);
        return View("CategoryForm", await provider.Hydrate(new(name, string.Empty, category.Value.IconName, false)));
    }


    [Route("submit")]
    public async Task<IActionResult> Submit(
        [FromForm] CategoryCrudFormViewModel viewModel,
        [FromServices] ICategoryService categoryService,
        [FromServices] IViewModelProvider<CategoryCrudFormViewModel> provider)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", await provider.Hydrate(viewModel));
        }

        BackendApiHttpResponse<Models.Category> result = viewModel switch
        {
            var vm when vm.CategoryName is not null && vm.CategoryName != string.Empty  
                => await categoryService.UpdateSingle(
                    new() { CategoryName = vm.CategoryName!, IconName = viewModel.IconName },
                    new() { CategoryName = vm.NewValue, IconName = viewModel.IconName }),
            _ => await categoryService.CreateSingle(
                    new() { CategoryName = viewModel.NewValue, IconName = viewModel.IconName })
        };

        this.BuildResponseMessageForRedirect(result);

        return RedirectToAction("index");
    }

    [Route("delete")]
    public async Task<IActionResult> SubmitDelete(
        string categoryName,
        [FromForm] CategoryCrudFormViewModel viewModel,
        [FromServices] ICategoryService categoryService,
        [FromServices] IViewModelProvider<CategoryCrudFormViewModel> provider)
    {
        if (!ModelState.IsValid)
        {
            return View("CategoryForm", viewModel);
        }

        var result = await categoryService.Delete(new() { CategoryName = categoryName });
        this.BuildResponseMessageForRedirect(result, this.CreateMessageDictionary());

        return RedirectToAction("index");
    }
}
