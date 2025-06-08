using FluentValidation;

namespace LiveMap.Application.SuggestedPoi.Validators;
using Domain.Models;

public class SuggestedPointOfInterestValidator : AbstractValidator<SuggestedPointOfInterest>
{
    public SuggestedPointOfInterestValidator()
    {
        RuleFor(poi => poi.Title)
            .MaximumLength(100)
            .WithMessage("Title should not be longer than 100 characters.");

        RuleFor(poi => poi.Description)
            .MaximumLength(1000)
            .WithMessage("Description should not be longer than 1000 characters.");
    }
}
