using FluentValidation;
using LiveMap.Application.Category.Requests;

namespace LiveMap.Application.Category.Validators
{
    public class CreateSingleValidator : AbstractValidator<CreateSingleRequest>
    {
        public CreateSingleValidator() 
        {
            RuleFor(request => request.Category).NotEmpty();
            RuleFor(request => request.Category).SetValidator(new CategoryValidator());
        }
    }
}
