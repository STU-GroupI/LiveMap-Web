using LiveMap.Domain.Models;
using LiveMap.Persistence;
using LiveMap.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LiveMapDashboard.Tests.Perstistance.RepositoryTests;

public class CategoryRepositoryTests : TestBase
{
    private static Category CreateCategory(string name = "Test", string icon = "icon.png")
        => new Category { CategoryName = name, IconName = icon };

    [Fact]
    public async Task Create_Should_Add_Category_And_Return_It()
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory();
        var result = await repo.Create(category);

        result.ShouldNotBeNull();
        result.CategoryName.ShouldBe(category.CategoryName);
        (await Context.Categories.CountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Create_Should_Throw_When_Category_With_Same_Name_Exists()
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory();
        await repo.Create(category);

        // Try to add another with the same name
        var duplicate = CreateCategory();
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

    [Fact]
    public async Task GetSingle_Should_Return_Category_If_Exists()
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory();
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

    [Fact]
    public async Task Update_Should_Update_IconName_If_Name_Same()
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory();
        await repo.Create(category);

        var updated = await repo.Update(category.CategoryName, category.CategoryName, "newicon.png");
        updated.ShouldBeTrue();

        var result = await repo.GetSingle(category.CategoryName);
        result.ShouldNotBeNull();
        result.IconName.ShouldBe("newicon.png");
    }

    [Fact]
    public async Task Update_Should_Create_New_And_Delete_Old_If_Name_Changes()
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory();
        await repo.Create(category);

        var updated = await repo.Update(category.CategoryName, "NewName", "newicon.png");
        updated.ShouldBeTrue();

        (await repo.GetSingle(category.CategoryName)).ShouldBeNull();
        var newCat = await repo.GetSingle("NewName");
        newCat.ShouldNotBeNull();
        newCat.IconName.ShouldBe("newicon.png");
    }

    [Fact]
    public async Task Update_Should_Return_False_If_Category_Does_Not_Exist()
    {
        var repo = new CategoryRepository(Context);

        var updated = await repo.Update("NonExistent", "NewName", "icon.png");
        updated.ShouldBeFalse();
    }

    [Fact]
    public async Task Delete_Should_Remove_Category_If_Exists()
    {
        var repo = new CategoryRepository(Context);

        var category = CreateCategory();
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

    [Fact]
    public async Task GetMultiple_Should_Skip_And_Take_Correctly()
    {
        var repo = new CategoryRepository(Context);

        await repo.Create(CreateCategory("A", "a.png"));
        await repo.Create(CreateCategory("B", "b.png"));
        await repo.Create(CreateCategory("C", "c.png"));

        var result = await repo.GetMultiple(skip: 1, take: 1);
        result.Length.ShouldBe(1);
        result[0].CategoryName.ShouldBe("B");
    }
}