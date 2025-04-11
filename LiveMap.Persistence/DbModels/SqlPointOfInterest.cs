using LiveMap.Domain.Models;
using NetTopologySuite.Geometries;

namespace LiveMap.Persistence.DbModels;

public class SqlPointOfInterest
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Point Position { get; set; }

    public required string CategoryName { get; set; }
    public Category? Category { get; set; }

    public required bool IsWheelchairAccessible { get; set; } = false;

    public required string StatusName { get; set; }
    public PointOfInterestStatus? Status { get; set; }

    public required Guid MapId { get; set; }
    public SqlMap? Map { get; set; }

    public virtual ICollection<SqlOpeningHours> OpeningHours { get; set; }
}