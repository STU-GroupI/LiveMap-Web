using System.ComponentModel.DataAnnotations.Schema;

namespace LiveMap.Domain.Models;
public class Category
{
    public const string STORE = "Store";
    public const string INFORMATION = "Information";
    public const string FIRSTAID_AND_MEDICAL = "First-aid & Medical";
    public const string TRASH_BIN = "Trash bin";
    public const string PARKING = "Parking";
    public const string ENTERTAINMENT = "Entertainment";
    public const string EMPTY = "Empty";
    public required string CategoryName { get; set; }
    public string IconName { get; set; } = string.Empty;
    
    [NotMapped]
    public bool? InUse { get; set; }
}