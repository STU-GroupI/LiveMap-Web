using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LiveMapDashboard.Web.Models.Poi;
using Models = LiveMap.Domain.Models;


public sealed record PoiCrudformViewModel(
    string? Id,
    string Title,
    string Category,
    string? Image,
    string Description,
    bool IsWheelchairAccessible,
    string MapId,
    IFormFile? ImageFile,
    Models.Coordinate Coordinate,
    OpeningHoursViewModel[] OpeningHours,
    Models.Category[]? Categories) : IValidatableObject
{
    public static PoiCrudformViewModel Empty =>
        new PoiCrudformViewModel(
            Id: string.Empty,
            Title: string.Empty,
            Category: string.Empty,
            Image: null,
            Description: string.Empty,
            IsWheelchairAccessible: false,
            MapId: string.Empty,
            ImageFile: null,
            Coordinate: new(0, 0),
            OpeningHours: Enumerable.Repeat(OpeningHoursViewModel.Empty, 7).ToArray(),
            Categories: []);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(Title))
        {
            results.Add(new ValidationResult("The Title field is required", new[] { nameof(Title) }));
        }

        if (string.IsNullOrWhiteSpace(MapId) || !Guid.TryParse(MapId, out _))
        {
            results.Add(new ValidationResult("Park ID must be a valid GUID.", new[] { nameof(MapId) }));
        }

        if (Coordinate.Latitude == 0 && Coordinate.Longitude == 0)
        {
            results.Add(new ValidationResult("A valid location must be added.", new[] { nameof(Coordinate) }));
        }

        if (OpeningHours != null)
        {
            foreach (var openingHour in OpeningHours)
            {
                if (openingHour == null)
                {
                    results.Add(new ValidationResult("Each opening hour must be set.", new[] { nameof(OpeningHours) }));
                    continue;
                }

                if (!Regex.IsMatch(openingHour.Start, @"^[012345]\d\:[012345]\d$") || !Regex.IsMatch(openingHour.End, @"^[012345]\d\:[012345]\d$"))
                {
                    results.Add(new ValidationResult("Opening and closing hours must follow the format [HH:MM].", new[] { nameof(OpeningHours) }));
                }
            }
        }

        return results;
    }
}
