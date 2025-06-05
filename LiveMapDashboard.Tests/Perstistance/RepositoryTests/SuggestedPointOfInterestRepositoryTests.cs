using Bogus;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using Shouldly;

namespace LiveMapDashboard.Tests.Perstistance.RepositoryTests;

public class SuggestedPointOfInterestRepositoryTests : TestBase
{
    private static readonly string[] RequiredCategories = new[]
    {
        "TestCategory"
    };

    private static readonly string[] RequiredMapNames = new[]
    {
        "TestMap"
    };

    private static readonly string[] RequiredApprovalStatuses = new[]
    {
        ApprovalStatus.PENDING,
        ApprovalStatus.APPROVED,
        ApprovalStatus.REJECTED
    };

    private Guid _testMapId;

    protected override void SeedBaseData()
    {
        // Ensure ApprovalStatus table has required statuses
        foreach (var status in RequiredApprovalStatuses)
        {
            if (!Context.ApprovalStatuses.Any(s => s.Status == status))
            {
                Context.ApprovalStatuses.Add(new ApprovalStatus { Status = status });
            }
        }

        // Ensure Category table has required categories
        foreach (var category in RequiredCategories)
        {
            if (!Context.Categories.Any(c => c.CategoryName == category))
            {
                Context.Categories.Add(new Category { CategoryName = category, IconName = "icon.png" });
            }
        }

        // Ensure at least one Map exists
        if (!Context.Maps.Any(m => m.Name == RequiredMapNames[0]))
        {
            var map = new LiveMap.Persistence.DbModels.SqlMap
            {
                Id = Guid.NewGuid(),
                Name = RequiredMapNames[0],
                Border = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326)
                    .CreatePolygon(new[]
                    {
                        new NetTopologySuite.Geometries.Coordinate(0, 0),
                        new NetTopologySuite.Geometries.Coordinate(0, 1),
                        new NetTopologySuite.Geometries.Coordinate(1, 1),
                        new NetTopologySuite.Geometries.Coordinate(1, 0),
                        new NetTopologySuite.Geometries.Coordinate(0, 0)
                    }),
                Bounds = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326)
                    .CreatePolygon(new[]
                    {
                        new NetTopologySuite.Geometries.Coordinate(0, 0),
                        new NetTopologySuite.Geometries.Coordinate(0, 1),
                        new NetTopologySuite.Geometries.Coordinate(1, 1),
                        new NetTopologySuite.Geometries.Coordinate(1, 0),
                        new NetTopologySuite.Geometries.Coordinate(0, 0)
                    }),
                ImageUrl = null
            };
            Context.Maps.Add(map);
            _testMapId = map.Id;
        }
        else
        {
            _testMapId = Context.Maps.First(m => m.Name == RequiredMapNames[0]).Id;
        }

        Context.SaveChanges();
    }

    private SuggestedPointOfInterest CreateSuggestedPoi(Guid? id = null, string? title = null, string? category = null, Guid? mapId = null)
    {
        var faker = new Faker();
        return new SuggestedPointOfInterest
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? faker.Lorem.Sentence(3),
            Description = faker.Lorem.Paragraph(),
            // Always use a valid seeded category
            CategoryName = category ?? RequiredCategories[0],
            Coordinate = new(0, 0),
            // Always use a valid seeded mapId
            MapId = mapId ?? _testMapId,
            IsWheelchairAccessible = faker.Random.Bool()
        };
    }

    public static IEnumerable<object[]> CreateSuggestedPoiData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.ProductName(),
                    RequiredCategories[0] // Always use seeded category
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(CreateSuggestedPoiData))]
    public async Task CreateSuggestedPointOfInterest_Should_Add_And_Return_It(string title, string category)
    {
        var repo = new SuggestedPointOfInterestRepository(Context);
        var poi = CreateSuggestedPoi(title: title, category: category);
        var result = await repo.CreateSuggestedPointOfInterest(poi);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(poi.Title);
        result.CategoryName.ShouldBe(poi.CategoryName);
        result.MapId.ShouldBe(poi.MapId);
    }

    public static IEnumerable<object[]> ReadSinglePoiData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.ProductName(),
                    RequiredCategories[0] // Always use seeded category
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(ReadSinglePoiData))]
    public async Task ReadSingle_Should_Return_Poi_If_Exists(string title, string category)
    {
        var repo = new SuggestedPointOfInterestRepository(Context);
        var poi = await repo.CreateSuggestedPointOfInterest(CreateSuggestedPoi(title: title, category: category));
        var result = await repo.ReadSingle(poi.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(poi.Id);
    }


    public static IEnumerable<object[]> NotExistingPoiIds
    {
        get
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new object[] { Guid.NewGuid() };
            }
        }
    }

    [Theory]
    [MemberData(nameof(NotExistingPoiIds))]
    public async Task ReadSingle_Should_Return_Null_If_Not_Exists(Guid id)
    {
        var repo = new SuggestedPointOfInterestRepository(Context);
        var result = await repo.ReadSingle(id);
        result.ShouldBeNull();
    }


    public static IEnumerable<object[]> DeletePoiData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[] { faker.Commerce.ProductName(), RequiredCategories[0] };
            }
        }
    }

    [Theory]
    [MemberData(nameof(DeletePoiData))]
    public async Task DeleteWithoutCommitAsync_Should_Remove_Poi_If_Exists(string title, string category)
    {
        var repo = new SuggestedPointOfInterestRepository(Context);
        var poi = await repo.CreateSuggestedPointOfInterest(CreateSuggestedPoi(title: title, category: category));
        await repo.DeleteWithoutCommitAsync(poi.Id);
        await Context.SaveChangesAsync();

        var result = await repo.ReadSingle(poi.Id);
        result.ShouldBeNull();
    }


    [Theory]
    [MemberData(nameof(NotExistingPoiIds))]
    public async Task DeleteWithoutCommitAsync_Should_Not_Throw_If_Poi_Does_Not_Exist(Guid id)
    {
        var repo = new SuggestedPointOfInterestRepository(Context);
        await repo.DeleteWithoutCommitAsync(id);
        await Context.SaveChangesAsync();

        // Should not throw and nothing to assert, but let's check the count remains unchanged
        var count = Context.SuggestedPointsOfInterest.Count();
        count.ShouldBe(0);
    }
}