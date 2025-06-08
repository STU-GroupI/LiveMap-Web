using FluentValidation;
using LiveMap.Domain.Models;

namespace LiveMap.Application.Map.Validators;

public class CoordinateValidator : AbstractValidator<Coordinate>
{
    public CoordinateValidator()
    {
        RuleFor(coord => coord.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(coord => coord.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }
}
