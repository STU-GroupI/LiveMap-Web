using FluentValidation;

namespace LiveMap.Application.RequestForChange.Validators;
using Domain.Models;
using LiveMap.Application.PointOfInterest.Validators;
using LiveMap.Application.SuggestedPoi.Validators;

public class RequestForChangeValidator : AbstractValidator<RequestForChange>
{
    public RequestForChangeValidator()
    {
        RuleFor(rfc => rfc.SuggestedPoi!).SetValidator(new SuggestedPointOfInterestValidator()).When(rfc => rfc.SuggestedPoi != null);
        RuleFor(rfc => rfc.Poi!.Title).MaximumLength(30)
            .WithMessage("Title should not be longer than 30 characters.").When(rfc => rfc.Poi != null);
    }
}
