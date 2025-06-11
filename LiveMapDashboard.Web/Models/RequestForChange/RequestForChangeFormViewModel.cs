using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;
using System.ComponentModel.DataAnnotations;
using static NetTopologySuite.Geometries.Utilities.GeometryMapper;
using System.Text.RegularExpressions;

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
