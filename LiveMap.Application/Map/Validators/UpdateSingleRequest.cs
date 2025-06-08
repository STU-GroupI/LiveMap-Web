using FluentValidation;
using LiveMap.Application.Map.Requests;

namespace LiveMap.Application.Map.Validators;

public class UpdateSingleValidator : AbstractValidator<UpdateSingleRequest>
{
    public UpdateSingleValidator() 
    {
        RuleFor(request => request.Map).SetValidator(new MapValidator());
    }
}