using LiveMap.Domain.Models;
using NetTopologySuite.Geometries;

namespace LiveMap.Persistence.DbModels;
public class SqlSuggestedPointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Point Position { get; set; }

    public string? CategoryName { get; set; }
    public required Category Category { get; set; }

    public required bool IsWheelchairAccessible { get; set; } = false;

    public required Guid MapId { get; set; }
    public required SqlMap Map { get; set; }

    public Guid? RFCId { get; set; }
    public required SqlRequestForChange RFC { get; set; }
}