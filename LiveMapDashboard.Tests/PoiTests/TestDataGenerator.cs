using Bogus;
using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;

namespace LiveMapDashboard.Tests.PoiTests;

public static class TestDataGenerator
{
    public static Faker<PointOfInterest> GetPointOfInterestFaker(Map? map = null)
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

            .RuleFor(p => p.MapId, f => map?.Id ?? f.Random.Guid())
            .RuleFor(p => p.Map, (f, p) => map ?? new Map { Id = p.MapId, Name = f.Address.City() });
    }

    public static PointOfInterest GenerateSinglePointOfInterest()
    {
        return GetPointOfInterestFaker().Generate();
    }

    public static List<PointOfInterest> GenerateMultiplePointsOfInterest(int count = 10)
    {
        return GetPointOfInterestFaker().Generate(count);
    }

    private static Faker<Map> GetMapFaker()
    {
        return new Faker<Map>()
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Name, f => f.Lorem.Word())
            .RuleFor(m => m.Coordinate, f => new(
                f.Address.Latitude(),
                f.Address.Longitude()))
            .RuleFor(m => m.PointOfInterests, () => new List<PointOfInterest>())
            .RuleFor(m => m.Area, (f, m) => [
                new(f.Address.Latitude(),
                    f.Address.Longitude()),
                new(f.Address.Latitude(),
                    f.Address.Longitude()),
                new(f.Address.Latitude(),
                    f.Address.Longitude()),
                new(f.Address.Latitude(),
                    f.Address.Longitude()),
                new(f.Address.Latitude(),
                    f.Address.Longitude()),
                ]);
    }

    public static List<Map> GenerateMultipleMaps(int count = 10, bool withPois = true)
    {
        var mapFaker = GetMapFaker();
        var maps = new List<Map>(mapFaker.Generate(count));
        
        if(!withPois)
        {
            return maps;
        }

        foreach (var map in maps)
        {
            map.PointOfInterests = GetPointOfInterestFaker(map)
                .Generate(50);
        }

        maps[count - 1].PointOfInterests = new List<PointOfInterest>();

        return maps;
    }
}