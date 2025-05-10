using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Rfc;

public record RequestForChangeFormViewModel(
    RequestForChange Rfc,
    PoiCrudformViewModel CrudformViewModel
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return [];
    }
}
