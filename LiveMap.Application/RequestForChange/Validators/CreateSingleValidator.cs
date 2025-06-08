using FluentValidation;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Domain.Models;

namespace LiveMap.Application.RequestForChange.Validators;

public class CreateSingleValidator : AbstractValidator<CreateSingleRequest>
{
    public CreateSingleValidator()
    {
        RuleFor(request => request.Rfc).SetValidator(new RequestForChangeValidator());
    }
}
