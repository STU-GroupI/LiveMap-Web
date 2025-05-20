using Model = LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Map;
using Models = LiveMap.Domain.Models;

public sealed record MapListViewModel(
    int? Skip,
    int? Take,
    PaginatedResult<Model.Map> Result
    ) : IValidatableObject
{
    public static MapListViewModel Empty => new(null, null, PaginatedResult<Models.Map>.Default);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = [];
        return results;
    }
}
