using FluentValidation;
using LiveMap.Domain.Models;

namespace LiveMap.Application.Map.Validators;

public class CoordinateValidator : AbstractValidator<Coordinate>
{
    public CoordinateValidator()
    {
        // The fact is being acknowledged that these two should be switched.
        RuleFor(coord => coord.Latitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Latitude must be between -180 and 180.");

        RuleFor(coord => coord.Longitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Longitude must be between -90 and 90.");
    }
}
