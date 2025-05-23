using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiveMapDashboard.Tests.Perstistance.RepositoryTests;

public class PointOfInterestRepositoryTests : TestBase
{
    private static readonly string[] RequiredStatuses = new[]
    {
        PointOfInterestStatus.ACTIVE,
        PointOfInterestStatus.INACTIVE,
        PointOfInterestStatus.PENDING
    };

    private static readonly string[] RequiredCategories = new[]
    {
        "TestCategory"
    };

    private static readonly string[] RequiredMapNames = new[]
    {
        "TestMap"
    };

    private Guid _testMapId;

    protected override void SeedBaseData()
    {
        // Ensure PointOfInterestStatus table has required statuses
        foreach (var status in RequiredStatuses)
        {
            if (!Context.PoIStatusses.Any(s => s.Status == status))
            {
                Context.PoIStatusses.Add(new PointOfInterestStatus { Status = status });
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

    private PointOfInterest CreatePoi(Guid? id = null, string? title = null, string? category = null, Guid? mapId = null, string? status = null)
        => new PointOfInterest
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? $"TestPoi_{Guid.NewGuid()}",
            Description = "Test Description",
            CategoryName = category ?? "TestCategory",
            Coordinate = new(0, 0),
            MapId = mapId ?? _testMapId,
            StatusName = status ?? PointOfInterestStatus.ACTIVE,
            IsWheelchairAccessible = false
        };

    [Fact]
    public async Task Create_Should_Add_Poi_And_Return_It()
    {
        var repo = new PointOfInterestRepository(Context);

        var poi = CreatePoi();
        var result = await repo.Create(poi);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(poi.Title);
        result.CategoryName.ShouldBe(poi.CategoryName);
        result.StatusName.ShouldBe(PointOfInterestStatus.ACTIVE);
        result.MapId.ShouldBe(poi.MapId);
    }

    [Fact]
    public async Task GetSingle_Should_Return_Poi_If_Exists()
    {
        var repo = new PointOfInterestRepository(Context);

        var poi = await repo.Create(CreatePoi());
        var result = await repo.GetSingle(poi.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(poi.Id);
    }

    [Fact]
    public async Task GetSingle_Should_Return_Null_If_Not_Exists()
    {
        var repo = new PointOfInterestRepository(Context);

        var result = await repo.GetSingle(Guid.NewGuid());
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetMultiple_Should_Return_Paginated_Pois()
    {
        var repo = new PointOfInterestRepository(Context);

        var mapId = _testMapId;
        await repo.Create(CreatePoi(title: "A", mapId: mapId));
        await repo.Create(CreatePoi(title: "B", mapId: mapId));
        await repo.Create(CreatePoi(title: "C", mapId: mapId));

        var result = await repo.GetMultiple(mapId, skip: 1, take: 1);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result.First().Title.ShouldBe("B");
    }

    [Fact]
    public async Task Update_Should_Update_Existing_Poi()
    {
        var repo = new PointOfInterestRepository(Context);

        var poi = await repo.Create(CreatePoi());
        poi.Title = "Updated Title";
        poi.Description = "Updated Description";
        poi.StatusName = PointOfInterestStatus.INACTIVE;

        var result = await repo.Update(poi);

        result.ShouldNotBeNull();
        result.Title.ShouldBe("Updated Title");
        result.Description.ShouldBe("Updated Description");
        result.StatusName.ShouldBe(PointOfInterestStatus.INACTIVE);
    }

    [Fact]
    public async Task Update_Should_Return_Null_If_Poi_Does_Not_Exist()
    {
        var repo = new PointOfInterestRepository(Context);

        var poi = CreatePoi();
        var result = await repo.Update(poi);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteSingle_Should_Remove_Poi_If_Exists()
    {
        var repo = new PointOfInterestRepository(Context);

        var poi = await repo.Create(CreatePoi());
        var deleted = await repo.DeleteSingle(poi.Id);

        deleted.ShouldBeTrue();

        var result = await repo.GetSingle(poi.Id);
        result.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteSingle_Should_Return_False_If_Poi_Does_Not_Exist()
    {
        var repo = new PointOfInterestRepository(Context);

        var deleted = await repo.DeleteSingle(Guid.NewGuid());
        deleted.ShouldBeFalse();
    }
}