using LiveMapDashboard.Web.Models.Category;

namespace LiveMapDashboard.Web.Models.Providers;
public class CategoryCrudFormViewModelProvider : IViewModelProvider<CategoryCrudFormViewModel>
{
    public Task<CategoryCrudFormViewModel> Hydrate(CategoryCrudFormViewModel viewModel)
    {
        return Task.FromResult(viewModel);
    }

    public async Task<CategoryCrudFormViewModel> Provide()
    {
        return await Hydrate(new(string.Empty, string.Empty));
    }
}
