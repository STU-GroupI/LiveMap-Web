using FluentValidation;

namespace LiveMap.Application.PointOfInterest.Validators;
using Domain.Models;

public class PointOfInterestValidator : AbstractValidator<PointOfInterest>
{
    public PointOfInterestValidator()
    {
        RuleFor(poi => poi.Title)
            .MaximumLength(30)
            .WithMessage("Title should not be longer than 30 characters.");

        RuleFor(poi => poi.Description)
            .MaximumLength(1000)
            .WithMessage("Description should not be longer than 1000 characters.");

        RuleFor(poi => poi.Image)
            .Must(image => string.IsNullOrEmpty(image) || Uri.IsWellFormedUriString(image, UriKind.Absolute))
            .WithMessage("Image must be a valid absolute URL.");

        RuleForEach(poi => poi.OpeningHours)
            .Must(oh => oh.Start < oh.End)
            .WithMessage("Time of opening must be before closing time.");
    }
}
