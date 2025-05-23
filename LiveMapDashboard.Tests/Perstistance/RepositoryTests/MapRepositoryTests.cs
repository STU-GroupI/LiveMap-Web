using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LiveMapDashboard.Tests.Perstistance.RepositoryTests;

public class MapRepositoryTests : TestBase
{
    private static Map CreateDomainMap(string? name = null)
    {
        // Use a simple square for both Area and Bounds
        var area = new[]
        {
            new Coordinate(0, 0),
            new Coordinate(0, 1),
            new Coordinate(1, 1),
            new Coordinate(1, 0),
            new Coordinate(0, 0)
        };
        return new Map
        {
            Id = Guid.NewGuid(),
            Name = name ?? $"TestMap_{Guid.NewGuid()}",
            Area = area,
            Bounds = area,
            ImageUrl = null,
            PointOfInterests = []
        };
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Map_And_Return_It()
    {
        var repo = new MapRepository(Context);

        var map = CreateDomainMap();
        var result = await repo.CreateAsync(map);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(map.Name);

        var dbMap = await Context.Maps.FirstOrDefaultAsync(m => m.Id == map.Id);
        dbMap.ShouldNotBeNull();
        dbMap.Name.ShouldBe(map.Name);
        dbMap.Border.ShouldNotBeNull();
        dbMap.Bounds.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetSingle_Should_Return_Map_If_Exists()
    {
        var repo = new MapRepository(Context);

        var map = CreateDomainMap();
        // Use repository to create so mapping is correct
        var created = await repo.CreateAsync(map);

        var result = await repo.GetSingle(map.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(map.Id);
        result.Name.ShouldBe(map.Name);
    }

    [Fact]
    public async Task GetSingle_Should_Return_Null_If_Not_Exists()
    {
        var repo = new MapRepository(Context);

        var result = await repo.GetSingle(Guid.NewGuid());
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetMultiple_Should_Return_Paginated_Maps()
    {
        var repo = new MapRepository(Context);

        // Add 3 maps
        var map1 = await repo.CreateAsync(CreateDomainMap("A"));
        var map2 = await repo.CreateAsync(CreateDomainMap("B"));
        var map3 = await repo.CreateAsync(CreateDomainMap("C"));

        var result = await repo.GetMultiple(skip: 1, take: 1);

        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.TotalCount.ShouldBe(3);
        result.Items[0].Name.ShouldBe("B");
    }

    [Fact]
    public async Task Update_Should_Update_Existing_Map()
    {
        var repo = new MapRepository(Context);

        var map = await repo.CreateAsync(CreateDomainMap("Original"));
        var updatedMap = new Map
        {
            Id = map.Id,
            Name = "Updated",
            Area = map.Area,
            Bounds = map.Bounds,
            ImageUrl = "http://example.com/image.png",
            PointOfInterests = []
        };

        var result = await repo.Update(updatedMap);

        result.ShouldNotBeNull();
        result.Name.ShouldBe("Updated");
        result.ImageUrl.ShouldBe("http://example.com/image.png");

        var dbMap = await Context.Maps.FirstOrDefaultAsync(m => m.Id == map.Id);
        dbMap.ShouldNotBeNull();
        dbMap.Name.ShouldBe("Updated");
        dbMap.ImageUrl.ShouldBe("http://example.com/image.png");
    }

    [Fact]
    public async Task Update_Should_Return_Null_If_Map_Does_Not_Exist()
    {
        var repo = new MapRepository(Context);

        var updatedMap = CreateDomainMap("NonExistent");
        var result = await repo.Update(updatedMap);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task Delete_Should_Remove_Map_If_Exists()
    {
        var repo = new MapRepository(Context);

        var map = await repo.CreateAsync(CreateDomainMap());
        var deleted = await repo.Delete(map.Id);

        deleted.ShouldBeTrue();

        var dbMap = await Context.Maps.FirstOrDefaultAsync(m => m.Id == map.Id);
        dbMap.ShouldBeNull();
    }

    [Fact]
    public async Task Delete_Should_Return_False_If_Map_Does_Not_Exist()
    {
        var repo = new MapRepository(Context);

        var deleted = await repo.Delete(Guid.NewGuid());
        deleted.ShouldBeFalse();
    }
}