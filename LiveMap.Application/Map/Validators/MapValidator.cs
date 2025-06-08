using FluentValidation;

namespace LiveMap.Application.Map.Validators;
using Domain.Models;

public class MapValidator : AbstractValidator<Map>
{
    public MapValidator()
    {
        RuleFor(map => map.Name)
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Map name must contain only letters.");
        RuleFor(map => map.Name)
            .MaximumLength(30)
            .WithMessage("Map name should not be longer than 30 characters");

        RuleFor(map => map.Area)
            .Must(area => area.Length > 2)
            .WithMessage("Area must contain at least 3 coordinates (polygon).");
        RuleForEach(map => map.Area)
            .SetValidator(new CoordinateValidator());

        RuleFor(map => map.Bounds)
            .Must(bounds => bounds.Length == 4)
            .WithMessage("Bounds must contain exactly 4 coordinates (rectangle).");
        RuleForEach(map => map.Bounds)
            .SetValidator(new CoordinateValidator());

        RuleFor(map => map.ImageUrl)
            .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("ImageUrl must be a valid absolute URL.");
    }
}
