using LiveMap.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LiveMapDashboard.Web.Models.Poi;

public sealed record PoiCrudformViewModel(
    string Title, 
    string Category, 
    string Description, 
    bool IsWheelchairAccessible,
    string MapId, 
    Coordinate Coordinate,
    OpeningHoursViewModel[] OpeningHours,
    Category[]? Categories
    ) : IValidatableObject
{
    public static PoiCrudformViewModel Empty => 
        new PoiCrudformViewModel(
            Title: string.Empty,
            Category: string.Empty,
            Description: string.Empty,
            IsWheelchairAccessible: false,
            MapId: string.Empty,
            Coordinate: new(0, 0),
            OpeningHours: Enumerable.Repeat(OpeningHoursViewModel.Empty, 7).ToArray(),
            Categories: []);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (!string.IsNullOrWhiteSpace(Title) && !Regex.IsMatch(Title, @"^[a-zA-Z0-9\s\-_\&\(\)\[\]\{\}\.\,\!\@\#\$\%\^\*\+\=]+$"))
        {
            results.Add(new ValidationResult("Title can only contain alphanumeric characters and basic symbols.", new[] { nameof(Title) }));
        }

        var validCategories = new List<string> { "Food", "Entertainment", "Park", "Museum" };

        if (!string.IsNullOrWhiteSpace(Category) && !validCategories.Contains(Category))
        {
            results.Add(new ValidationResult("Category must be one of the valid predefined values.", new[] { nameof(Category) }));
        }
        
        if (string.IsNullOrWhiteSpace(MapId) || !Guid.TryParse(MapId, out _))
        {
            results.Add(new ValidationResult("Park ID must be a valid GUID.", new[] { nameof(MapId) }));
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
