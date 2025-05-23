using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
        => new SuggestedPointOfInterest
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? $"TestSuggestedPoi_{Guid.NewGuid()}",
            Description = "Test Description",
            CategoryName = category ?? "TestCategory",
            Coordinate = new(0, 0),
            MapId = mapId ?? _testMapId,
            IsWheelchairAccessible = false
        };

    [Fact]
    public async Task CreateSuggestedPointOfInterest_Should_Add_And_Return_It()
    {
        var repo = new SuggestedPointOfInterestRepository(Context);

        var poi = CreateSuggestedPoi();
        var result = await repo.CreateSuggestedPointOfInterest(poi);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(poi.Title);
        result.CategoryName.ShouldBe(poi.CategoryName);
        result.MapId.ShouldBe(poi.MapId);
    }

    [Fact]
    public async Task ReadSingle_Should_Return_Poi_If_Exists()
    {
        var repo = new SuggestedPointOfInterestRepository(Context);

        var poi = await repo.CreateSuggestedPointOfInterest(CreateSuggestedPoi());
        var result = await repo.ReadSingle(poi.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(poi.Id);
    }

    [Fact]
    public async Task ReadSingle_Should_Return_Null_If_Not_Exists()
    {
        var repo = new SuggestedPointOfInterestRepository(Context);

        var result = await repo.ReadSingle(Guid.NewGuid());
        result.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteWithoutCommitAsync_Should_Remove_Poi_If_Exists()
    {
        var repo = new SuggestedPointOfInterestRepository(Context);

        var poi = await repo.CreateSuggestedPointOfInterest(CreateSuggestedPoi());
        await repo.DeleteWithoutCommitAsync(poi.Id);
        await Context.SaveChangesAsync();

        var result = await repo.ReadSingle(poi.Id);
        result.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteWithoutCommitAsync_Should_Not_Throw_If_Poi_Does_Not_Exist()
    {
        var repo = new SuggestedPointOfInterestRepository(Context);

        var id = Guid.NewGuid();
        await repo.DeleteWithoutCommitAsync(id);
        await Context.SaveChangesAsync();

        // Should not throw and nothing to assert, but let's check the count remains unchanged
        var count = Context.SuggestedPointsOfInterest.Count();
        count.ShouldBe(0);
    }
}