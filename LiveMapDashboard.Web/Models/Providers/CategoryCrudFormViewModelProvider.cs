using LiveMapDashboard.Web.Models.Category;

namespace LiveMapDashboard.Web.Models.Providers;
public class CategoryCrudFormViewModelProvider : IViewModelProvider<CategoryCrudFormViewModel>
{
    public async Task<CategoryCrudFormViewModel> Hydrate(CategoryCrudFormViewModel viewModel)
    {
        return viewModel;
    }

    public async Task<CategoryCrudFormViewModel> Provide()
    {
        return await Hydrate(new(string.Empty, string.Empty));
    }
}
