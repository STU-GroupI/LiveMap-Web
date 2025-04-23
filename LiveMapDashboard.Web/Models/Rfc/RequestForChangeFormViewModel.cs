using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Rfc;

public record RequestForChangeFormViewModel(
    RequestForChange Rfc,
    PoiCrudformViewModel CrudformViewModel
);