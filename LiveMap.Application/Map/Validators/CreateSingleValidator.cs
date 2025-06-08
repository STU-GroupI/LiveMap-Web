using FluentValidation;
using LiveMap.Application.Map.Requests;

namespace LiveMap.Application.Map.Validators;

public class CreateSingleValidator : AbstractValidator<CreateSingleRequest>
{
    public CreateSingleValidator() 
    {
        RuleFor(request => request.Map).SetValidator(new MapValidator());
    }
}
