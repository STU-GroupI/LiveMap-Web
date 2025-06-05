using LiveMap.Domain.Models;
using LiveMap.Persistence;
using LiveMap.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Bogus;

namespace LiveMapDashboard.Tests.Perstistance.RepositoryTests;

public class CategoryRepositoryTests : TestBase
{
    private static Category CreateCategory(string name = "Test", string icon = "icon.png")
        => new Category { CategoryName = name, IconName = icon };


    public static IEnumerable<object[]> CreateCategoryData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.System.FileName("png")
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(CreateCategoryData))]
    public async Task Create_Should_Add_Category_And_Return_It(string name, string icon)
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory(name, icon);
        var result = await repo.Create(category);

        result.ShouldNotBeNull();
        result.CategoryName.ShouldBe(category.CategoryName);
        (await Context.Categories.CountAsync()).ShouldBe(1);
    }

    public static IEnumerable<object[]> DuplicateCategoryData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                var name = faker.Commerce.Department() + faker.Random.Number(10000);
                var icon = faker.System.FileName("png");
                yield return new object[] { name, icon };
            }
        }
    }

    [Theory]
    [MemberData(nameof(DuplicateCategoryData))]
    public async Task Create_Should_Throw_When_Category_With_Same_Name_Exists(string name, string icon)
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory(name, icon);
        await repo.Create(category);

        // Try to add another with the same name
        var duplicate = CreateCategory(name, icon);
        await Should.ThrowAsync<InvalidOperationException>(async () => await repo.Create(duplicate));
    }

    [Fact]
    public async Task Create_Should_Throw_DbUpdateException_When_Database_Already_Has_Category_With_Same_Name()
    {
        // Insert a category directly using a separate context
        var existingCategory = CreateCategory("Duplicate", "icon1.png");

        var connectionString = Context.Database.GetConnectionString();

        var options = new DbContextOptionsBuilder<LiveMapContext>()
            .UseSqlServer(connectionString, x => x.UseNetTopologySuite())
            .Options;

        using (var directContext = new LiveMapContext(options))
        {
            directContext.Categories.Add(existingCategory);
            await directContext.SaveChangesAsync();
        }

        // Now, in a new context, try to add a category with the same name
        using (var testContext = new LiveMapContext(options))
        {
            var repo = new CategoryRepository(testContext);
            var duplicate = CreateCategory("Duplicate", "icon2.png");
            await Should.ThrowAsync<DbUpdateException>(async () => await repo.Create(duplicate));
        }
    }

    public static IEnumerable<object[]> GetSingleCategoryData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.System.FileName("png")
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetSingleCategoryData))]
    public async Task GetSingle_Should_Return_Category_If_Exists(string name, string icon)
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory(name, icon);
        await repo.Create(category);

        var result = await repo.GetSingle(category.CategoryName);

        result.ShouldNotBeNull();
        result.CategoryName.ShouldBe(category.CategoryName);
    }

    [Fact]
    public async Task GetSingle_Should_Return_Null_If_Not_Exists()
    {
        var repo = new CategoryRepository(Context);

        var result = await repo.GetSingle("NonExistent");
        result.ShouldBeNull();
    }

    public static IEnumerable<object[]> UpdateIconNameData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.System.FileName("png"),
                    faker.System.FileName("png")
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(UpdateIconNameData))]
    public async Task Update_Should_Update_IconName_If_Name_Same(string name, string oldIcon, string newIcon)
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory(name, oldIcon);
        await repo.Create(category);

        var updated = await repo.Update(category.CategoryName, category.CategoryName, newIcon);
        updated.ShouldBeTrue();

        var result = await repo.GetSingle(category.CategoryName);
        result.ShouldNotBeNull();
        result.IconName.ShouldBe(newIcon);
    }

    public static IEnumerable<object[]> UpdateNameAndIconData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.System.FileName("png"),
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.System.FileName("png")
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(UpdateNameAndIconData))]
    public async Task Update_Should_Create_New_And_Delete_Old_If_Name_Changes(string oldName, string oldIcon, string newName, string newIcon)
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory(oldName, oldIcon);
        await repo.Create(category);

        var updated = await repo.Update(category.CategoryName, newName, newIcon);
        updated.ShouldBeTrue();

        (await repo.GetSingle(oldName)).ShouldBeNull();
        var newCat = await repo.GetSingle(newName);
        newCat.ShouldNotBeNull();
        newCat.IconName.ShouldBe(newIcon);
    }

    [Fact]
    public async Task Update_Should_Return_False_If_Category_Does_Not_Exist()
    {
        var repo = new CategoryRepository(Context);

        var updated = await repo.Update("NonExistent", "NewName", "icon.png");
        updated.ShouldBeFalse();
    }

    public static IEnumerable<object[]> DeleteCategoryData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.Department() + faker.Random.Number(10000),
                    faker.System.FileName("png")
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(DeleteCategoryData))]
    public async Task Delete_Should_Remove_Category_If_Exists(string name, string icon)
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory(name, icon);
        await repo.Create(category);

        var deleted = await repo.Delete(category.CategoryName);
        deleted.ShouldBeTrue();

        (await repo.GetSingle(category.CategoryName)).ShouldBeNull();
    }

    [Fact]
    public async Task Delete_Should_Return_False_If_Category_Does_Not_Exist()
    {
        var repo = new CategoryRepository(Context);

        var deleted = await repo.Delete("NonExistent");
        deleted.ShouldBeFalse();
    }

    public static IEnumerable<object[]> GetMultipleData =>
        new List<object[]>
        {
            new object[] { 1, 1, new[] { "A", "B", "C" }, new[] { "a.png", "b.png", "c.png" }, "B" },
            new object[] { 0, 2, new[] { "X", "Y", "Z" }, new[] { "x.png", "y.png", "z.png" }, "X" }
        };

    [Theory]
    [MemberData(nameof(GetMultipleData))]
    public async Task GetMultiple_Should_Skip_And_Take_Correctly(int skip, int take, string[] names, string[] icons, string expectedName)
    {
        var repo = new CategoryRepository(Context);

        for (int i = 0; i < names.Length; i++)
        {
            await repo.Create(CreateCategory(names[i], icons[i]));
        }

        var result = await repo.GetMultiple(skip: skip, take: take);
        result.Length.ShouldBe(take);
        result[0].CategoryName.ShouldBe(expectedName);
    }
}