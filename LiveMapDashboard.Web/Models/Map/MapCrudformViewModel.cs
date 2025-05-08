using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Map;
using Models = LiveMap.Domain.Models;

public sealed record MapCrudformViewModel(
    string? Id,
    string Name,
    Models.Coordinate[] Area,
    Models.Coordinate Coordinate
    )
{
    public static MapCrudformViewModel Empty => new(
        Id: string.Empty,
        Name: string.Empty,
        Area: [],
        Coordinate: new(0.0, 0.0)
        );

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // TODO: write validation

        return results;
    }
}