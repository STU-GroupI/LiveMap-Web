using FluentValidation;
using LiveMap.Application.SuggestedPoi.Requests;

namespace LiveMap.Application.SuggestedPoi.Validators;

public class CreateSingleValidator : AbstractValidator<CreateSingleRequest>
{
    public CreateSingleValidator()
    {
        RuleFor(request => request.SuggestedPoi).SetValidator(new SuggestedPointOfInterestValidator());
    }
}
