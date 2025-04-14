namespace LiveMap.Domain.Models;
public class Category
{
    //List<Category> categories = [
    //        new() { CategoryName = "Store" },
    //        new() { CategoryName = "Information" },
    //        new() { CategoryName = "First-aid & Medical" },
    //        new() { CategoryName = "Trash bin" },
    //        new() { CategoryName = "Parking" },
    //        new() { CategoryName = "Entertainment" },
    //        new() { CategoryName = "EMPTY" },
    //    ];
    public const string STORE = "Store";
    public const string INFORMATION = "Information";
    public const string FIRSTAID_AND_MEDICAL = "First-aid & Medical";
    public const string TRASH_BIN = "Trash bin";
    public const string PARKING = "Parking";
    public const string ENTERTAINMENT = "Entertainment";
    public const string EMPTY = "Empty";
    public required string CategoryName { get; set; }
}