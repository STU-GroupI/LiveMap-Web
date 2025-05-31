using FluentValidation;
namespace LiveMap.Application.Category.Validators;
using Domain.Models;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator() 
    {
        RuleFor(category => category.CategoryName).NotEmpty();
        RuleFor(category => category.CategoryName)
            .Matches("^[a-zA-Z0-9]+$")
            .WithMessage("Category name must contain only letters and numbers.");
        RuleFor(category => category.CategoryName)
            .MaximumLength(30)
            .WithMessage("Category name should not be longer than 30 characters");
    }
}
