using FluentValidation;
using LiveMap.Application.Category.Requests;

namespace LiveMap.Application.Category.Validators
{
    public class UpdateSingleValidator : AbstractValidator<UpdateSingleRequest>
    {
        public UpdateSingleValidator() 
        {
            RuleFor(request => request.newName)
                .Matches("^[a-zA-Z0-9&]+$")
                .WithMessage("Category can only contain letters, numbers, and '&' symbols.");
            RuleFor(request => request.newName)
                .MaximumLength(30)
                .WithMessage("Category name should not be longer than 30 characters");
        }
    }
}
