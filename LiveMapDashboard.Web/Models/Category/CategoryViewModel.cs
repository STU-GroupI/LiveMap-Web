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
    int? Skip,
    int? Take,
    Category[] Categories
) : IValidatableObject
{
    /// <summary>
    /// An empty view model instance with no category name.
    /// </summary>
    public static CategoryViewModel Empty => new(Skip: null,
            Take: null, Categories: []);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = [];
        return results;
    }
}
