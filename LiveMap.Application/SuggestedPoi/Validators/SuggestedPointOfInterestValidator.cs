using FluentValidation;

namespace LiveMap.Application.SuggestedPoi.Validators;
using Domain.Models;

public class SuggestedPointOfInterestValidator : AbstractValidator<SuggestedPointOfInterest>
{
    public SuggestedPointOfInterestValidator()
    {
        RuleFor(sugpoi => sugpoi.Title)
            .Matches("^[a-zA-Z0-9\\s\\-_\\&\\(\\)\\[\\]\\{\\}\\.\\,\\!\\@\\#\\$\\%\\^\\*\\+\\=]+$")
            .WithMessage("Title can only contain alphanumeric characters and basic symbols.");
        RuleFor(sugpoi => sugpoi.Title)
            .MaximumLength(100)
            .WithMessage("Title should not be longer than 50 characters.");

        RuleFor(sugpoi => sugpoi.Description)
            .MaximumLength(1000)
            .WithMessage("Description should not be longer than 1000 characters.");
    }
}
