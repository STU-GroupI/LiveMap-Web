namespace LiveMap.Domain.Models;
public class PointOfInterestStatus
{
    public const string ACTIVE = "Active";
    public const string INACTIVE = "Inactive";
    public const string PENDING = "Pending";

    public required string Status { get; set; }
}
