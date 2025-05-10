using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LiveMapDashboard.Web.Models.Map;
using Models = LiveMap.Domain.Models;

public sealed record MapCrudformViewModel(
    string? Id,
    string Name,
    string? ImageUrl,
    Models.Coordinate TopLeft,
    Models.Coordinate TopRight,
    Models.Coordinate BottomLeft,
    Models.Coordinate BottomRight,
    Models.Coordinate Coordinate
    )
{
    public static MapCrudformViewModel Empty => new(
        Id: string.Empty,
        Name: string.Empty,
        ImageUrl: string.Empty,

        TopLeft: new(0.0, 0.0),
        TopRight: new(0.0, 0.0),
        BottomLeft: new(0.0, 0.0),
        BottomRight: new(0.0, 0.0),

        Coordinate: new(0.0, 0.0)
        );

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        
        // if (string.IsNullOrWhiteSpace(Name))
        // {
        //     results.Add(new ValidationResult("The Name field is required", new[] { nameof(Name) }));
        // }
        //
        // if (!string.IsNullOrWhiteSpace(Name) && !Regex.IsMatch(Name, @"^[a-zA-Z0-9\s\-_\&\(\)\[\]\{\}\.\,\!\@\#\$\%\^\*\+\=]+$"))
        // {
        //     results.Add(new ValidationResult("Name can only contain alphanumeric characters and basic symbols.", new[] { nameof(Name) }));
        // }
        //
        // if (Coordinate.Latitude == 0 && Coordinate.Longitude == 0)
        // {
        //     results.Add(new ValidationResult("A valid location must be added.", new[] { nameof(Coordinate) }));
        // }
        
        return results;
    }
}