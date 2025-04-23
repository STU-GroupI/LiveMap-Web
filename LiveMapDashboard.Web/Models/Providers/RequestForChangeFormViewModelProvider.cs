using LiveMapDashboard.Web.Models.Rfc;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class RequestForChangeFormViewModelProvider : IViewModelProvider<RequestForChangeFormViewModel>
    {
        public Task<RequestForChangeFormViewModel> Hydrate(RequestForChangeFormViewModel viewModel)
        {
            // get RFC

            // if RFC has POI, get THAT one

            // if RFC has suggested POI, get THAT one

            // get approval statusses

            // get categories
        }

        public Task<RequestForChangeFormViewModel> Provide()
        {
            throw new NotImplementedException();
        }
    }
}
