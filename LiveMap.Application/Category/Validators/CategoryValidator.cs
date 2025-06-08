using FluentValidation;

namespace LiveMap.Application.Category.Validators;
using Domain.Models;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(category => category.CategoryName)
            .Matches("^[a-zA-Z0-9& ]+$")
            .WithMessage("Category can only contain letters, numbers, and '&' symbols.");
        RuleFor(category => category.CategoryName)
            .MaximumLength(30)
            .WithMessage("Category should not be longer than 30 characters");
    }
}
