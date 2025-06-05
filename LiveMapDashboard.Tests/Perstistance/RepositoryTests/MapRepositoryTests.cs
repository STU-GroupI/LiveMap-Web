using Bogus;
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

    public static IEnumerable<object[]> CreateMapData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000)
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(CreateMapData))]
    public async Task CreateAsync_Should_Add_Map_And_Return_It(string name)
    {
        var repo = new MapRepository(Context);

        var map = CreateDomainMap(name);
        var result = await repo.CreateAsync(map);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(map.Name);

        var dbMap = await Context.Maps.FirstOrDefaultAsync(m => m.Id == map.Id);
        dbMap.ShouldNotBeNull();
        dbMap.Name.ShouldBe(map.Name);
        dbMap.Border.ShouldNotBeNull();
        dbMap.Bounds.ShouldNotBeNull();
    }

    public static IEnumerable<object[]> GetSingleMapData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000)
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetSingleMapData))]
    public async Task GetSingle_Should_Return_Map_If_Exists(string name)
    {
        var repo = new MapRepository(Context);

        var map = CreateDomainMap(name);
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

    public static IEnumerable<object[]> GetMultipleMapData
    {
        get
        {
            // Generate 3 sets of map names for pagination
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                var names = new[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.Commerce.Department() + faker.Random.Number(10000)
                };
                yield return new object[] { 1, 1, names, names[1] };
                yield return new object[] { 0, 2, names, names[0] };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetMultipleMapData))]
    public async Task GetMultiple_Should_Return_Paginated_Maps(int skip, int take, string[] names, string expectedName)
    {
        var repo = new MapRepository(Context);

        // Add 3 maps
        foreach (var name in names)
        {
            await repo.CreateAsync(CreateDomainMap(name));
        }

        var result = await repo.GetMultiple(skip: skip, take: take);

        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(take);
        result.TotalCount.ShouldBe(names.Length);
        result.Items[0].Name.ShouldBe(expectedName);
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