using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LiveMapDashboard.Web.Models.Suggestion;

public sealed record PoiSuggestionsViewModel(
    Guid MapId,
    int? Skip,
    int? Take,
    bool? Ascending,
    SuggestedPointOfInterest[] SuggestedPointOfInterests
    ) : IValidatableObject
{
    public static PoiSuggestionsViewModel Empty =>
        new(
            MapId: Guid.Empty,
            Skip: null,
            Take: null,
            Ascending: null,
            SuggestedPointOfInterests: []
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
