using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Rfc;

public sealed record RFCsViewModel(
    Guid MapId,
    int? Skip,
    int? Take,
    bool? Ascending,
    RequestForChange[] RFCs
    ) : IValidatableObject
{
    public static RFCsViewModel Empty =>
        new(
            MapId: Guid.Empty,
            Skip: null,
            Take: null,
            Ascending: null,
            RFCs: []
            );

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = [];
        return results;
    }

    public static string ToDateTime(DateTime? dateTime)
    {
        if (dateTime == null)
        {
            return "Geen Datum";
        }
        else
        {
            return dateTime.Value.ToString("d MMMM yyyy HH:mm", new System.Globalization.CultureInfo("nl-nl"));
        }
    }
}
