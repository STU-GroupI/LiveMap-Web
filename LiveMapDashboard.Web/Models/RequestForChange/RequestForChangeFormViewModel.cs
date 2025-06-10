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
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(CrudformViewModel.Title))
        {
            results.Add(new ValidationResult("The Title field is required", new[] { nameof(CrudformViewModel.Title) }));
        }

        if (string.IsNullOrWhiteSpace(CrudformViewModel.MapId) || !Guid.TryParse(CrudformViewModel.MapId, out _))
        {
            results.Add(new ValidationResult("Park ID must be a valid GUID.", new[] { nameof(CrudformViewModel.MapId) }));
        }

        if (CrudformViewModel.Coordinate.Latitude == 0 && CrudformViewModel.Coordinate.Longitude == 0)
        {
            results.Add(new ValidationResult("A valid location must be added.", new[] { nameof(CrudformViewModel.Coordinate) }));
        }

        var openingsHours = CrudformViewModel.OpeningHours;
        if (openingsHours != null)
        {
            foreach (var openingHour in openingsHours)
            {
                if (openingHour == null)
                {
                    results.Add(new ValidationResult("Each opening hour must be set.", new[] { nameof(openingsHours) }));
                    continue;
                }

                if (!Regex.IsMatch(openingHour.Start, @"^[012345]\d\:[012345]\d$") || !Regex.IsMatch(openingHour.End, @"^[012345]\d\:[012345]\d$"))
                {
                    results.Add(new ValidationResult("Opening and closing hours must follow the format [HH:MM].", new[] { nameof(openingsHours) }));
                }
            }
        }

        return results;
    }
}
