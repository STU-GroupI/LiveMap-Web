namespace LiveMapDashboard.Web.Models.Providers
{
    public interface IViewModelProvider<T>
    {
        Task<T> Provide();
        Task<T> Hydrate(T viewModel);
    }
}
