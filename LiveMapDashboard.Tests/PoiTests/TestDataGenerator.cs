using Bogus;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.PoiTests;

public static class TestDataGenerator
{
    public static Faker<PointOfInterest> GetPointOfInterestFaker()
    {
        return new Faker<PointOfInterest>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Title, f => f.Lorem.Sentence(3))
            .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
            .RuleFor(p => p.Coordinate, f => new Coordinate(
                f.Address.Latitude(), 
                f.Address.Longitude()))
            
            .RuleFor(p => p.CategoryName, f => f.Commerce.Department())
            .RuleFor(p => p.Category, (f, p) => new Category { CategoryName = p.CategoryName })
            
            .RuleFor(p => p.StatusName, f => f.PickRandom(new[] { "Active", "Inactive", "Pending" }))
            .RuleFor(p => p.Status, (f, p) => new PointOfInterestStatus { Status = p.StatusName })
            
            .RuleFor(p => p.MapId, f => f.Random.Guid())
            .RuleFor(p => p.Map, f => new Map { Name = f.Address.City() });
    }

    public static PointOfInterest GenerateSinglePointOfInterest()
    {
        return GetPointOfInterestFaker().Generate();
    }

    public static List<PointOfInterest> GenerateMultiplePointsOfInterest(int count = 10)
    {
        return GetPointOfInterestFaker().Generate(count);
    }
}