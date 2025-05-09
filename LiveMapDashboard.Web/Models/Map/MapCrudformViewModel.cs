using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Map;
using Models = LiveMap.Domain.Models;

public sealed record MapCrudformViewModel(
    string? Id,
    string Name,
    string? Image,
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
        Image: string.Empty,

        TopLeft: new(0.0, 0.0),
        TopRight: new(0.0, 0.0),
        BottomLeft: new(0.0, 0.0),
        BottomRight: new(0.0, 0.0),

        Coordinate: new(0.0, 0.0)
        );

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // TODO: write validation

        return results;
    }
}