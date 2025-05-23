using Bogus;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.Deprecated.PoiTests;

public static class TestDataGenerator
{
    public static Faker<PointOfInterest> GetPointOfInterestFaker(Map? map = null)
    {
        #pragma warning disable CS8601
    
        return new Faker<PointOfInterest>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Title, f => f.Lorem.Sentence(3))
            .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
            .RuleFor(p => p.Image, f => f.PickRandom(new[] { "https://placehold.co/960x565", null }))
            .RuleFor(p => p.Coordinate, f => new Coordinate(
                f.Address.Latitude(),
                f.Address.Longitude()))
            .RuleFor(p => p.CategoryName, f => f.Commerce.Department())
            .RuleFor(p => p.Category, (f, p) => new Category { CategoryName = p.CategoryName })
            .RuleFor(p => p.StatusName, f => f.PickRandom(new[] { "Active", "Inactive", "Pending" }))
            .RuleFor(p => p.Status, (f, p) => new PointOfInterestStatus { Status = p.StatusName })
            .RuleFor(p => p.MapId, f => map?.Id ?? f.Random.Guid())
            .RuleFor(p => p.Map, (f, p) => map ?? new Map
            {
                Id = p.MapId,
                Name = f.Address.City(),
                PointOfInterests = new List<PointOfInterest>(),
                Area = Array.Empty<Coordinate>(),
                Bounds = Array.Empty<Coordinate>(),
                ImageUrl = f.Image.PicsumUrl()
            });
    
        #pragma warning restore CS8601
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
                ])
            .RuleFor(m => m.Bounds, (f, m) => [
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

        if (!withPois)
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