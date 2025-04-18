using LiveMap.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Categories;

/// <summary>
/// ViewModel representing a single Category for display and binding purposes.
/// </summary>
public sealed record CategoryViewModel(
    /// <summary>
    /// The name of the category. Should correspond to a value stored in the database.
    /// </summary>
    string CategoryName
) : IValidatableObject
{
    /// <summary>
    /// An empty view model instance with no category name.
    /// </summary>
    public static CategoryViewModel Empty => new(string.Empty);

    /// <summary>
    /// Ensures the category name is provided.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(CategoryName))
        {
            yield return new ValidationResult(
                "Category name is required.",
                new[] { nameof(CategoryName) }
            );
        }
    }
}
