using Model = LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Park;

public sealed record MapListViewModel(
    int? Skip,
    int? Take,
    PaginatedResult<Model.Map> Result
    ) : IValidatableObject
{
    public static MapListViewModel Empty => new(null, null, PaginatedResult<Model.Map>.Default);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = [];
        return results;
    }
}
