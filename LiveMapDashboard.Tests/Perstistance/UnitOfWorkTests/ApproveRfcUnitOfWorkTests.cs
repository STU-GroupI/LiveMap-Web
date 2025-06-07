using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Extensions;
using LiveMap.Persistence.UnitsOfWork.RequestForChange;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LiveMapDashboard.Tests.Perstistance.UnitOfWorkTests;

public class ApproveRfcUnitOfWorkTests : TestBase
{
    private Guid _testMapId;

    protected override void SeedBaseData()
    {
        // Approval statuses
        foreach (var status in new[] { ApprovalStatus.PENDING, ApprovalStatus.APPROVED, ApprovalStatus.REJECTED })
        {
            if (!Context.ApprovalStatuses.Any(s => s.Status == status))
                Context.ApprovalStatuses.Add(new ApprovalStatus { Status = status });
        }

        // PointOfInterest statuses
        foreach (var status in new[] { PointOfInterestStatus.ACTIVE, PointOfInterestStatus.INACTIVE, PointOfInterestStatus.PENDING })
        {
            if (!Context.PoIStatusses.Any(s => s.Status == status))
                Context.PoIStatusses.Add(new PointOfInterestStatus { Status = status });
        }

        // Categories
        foreach (var cat in new[] { Category.STORE, Category.INFORMATION, Category.FIRSTAID_AND_MEDICAL, Category.TRASH_BIN, Category.PARKING, Category.ENTERTAINMENT, Category.EMPTY })
        {
            if (!Context.Categories.Any(c => c.CategoryName == cat))
                Context.Categories.Add(new Category { CategoryName = cat, IconName = cat + "Icon" });
        }

        // Map (domain model, not SQL type)
        var map = new Map
        {
            Id = Guid.NewGuid(),
            Name = "TestMap",
            Area = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(0, 1),
                new Coordinate(1, 1),
                new Coordinate(1, 0),
                new Coordinate(0, 0)
            },
            Bounds = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(0, 1),
                new Coordinate(1, 1),
                new Coordinate(1, 0),
                new Coordinate(0, 0)
            },
            ImageUrl = null,
            PointOfInterests = []
        };
        Context.Maps.Add(map.ToSqlMap());
        _testMapId = map.Id;

        Context.SaveChanges();
    }


    // Data generators for [Theory] tests
    public static IEnumerable<object[]> PoiData
    {
        get
        {
            var faker = new Bogus.Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.ProductName(),
                    Category.STORE, // Always use seeded category
                    PointOfInterestStatus.PENDING // Always use seeded status
                };
            }
        }
    }

    private PointOfInterest CreatePoi(Guid? id = null, string? title = null, string? category = null, string? status = null)
        => new PointOfInterest
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? $"TestPoi_{Guid.NewGuid()}",
            Description = "Test Description",
            CategoryName = category ?? Category.STORE,
            Coordinate = new Coordinate(0.5, 0.5),
            MapId = _testMapId,
            StatusName = status ?? PointOfInterestStatus.PENDING,
            IsWheelchairAccessible = false
        };


    public static IEnumerable<object[]> SuggestedPoiData
    {
        get
        {
            var faker = new Bogus.Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[]
                {
                    faker.Commerce.ProductName(),
                    Category.STORE // Always use seeded category
                };
            }
        }
    }

    private SuggestedPointOfInterest CreateSuggestedPoi(Guid? id = null, string? title = null, string? category = null)
        => new SuggestedPointOfInterest
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? $"TestSuggestedPoi_{Guid.NewGuid()}",
            Description = "Test Description",
            CategoryName = category ?? Category.STORE,
            Coordinate = new Coordinate(0.5, 0.5),
            MapId = _testMapId,
            IsWheelchairAccessible = false
        };

    // Helper: Create a RequestForChange (domain model)
    private RequestForChange CreateRfc(Guid? id = null, Guid? poiId = null, Guid? suggestedPoiId = null, string? status = null)
        => new RequestForChange
        {
            Id = id ?? Guid.NewGuid(),
            PoiId = poiId,
            SuggestedPoiId = suggestedPoiId,
            ApprovalStatus = status ?? ApprovalStatus.PENDING,
            SubmittedOn = DateTime.UtcNow,
            Message = "Test RFC"
        };


    [Theory]
    [MemberData(nameof(SuggestedPoiData))]
    public async Task CommitAsync_Should_Approve_Rfc_With_SuggestedPoi(string title, string category)
    {
        // Arrange
        var uow = new ApproveRfcUnitOfWork(Context);

        // 1. Create and save the suggested POI
        var suggestedPoi = CreateSuggestedPoi(title: title, category: category);
        var sqlSuggestedPoi = suggestedPoi.ToSqlSuggestedPointOfInterest();
        Context.SuggestedPointsOfInterest.Add(sqlSuggestedPoi);
        await Context.SaveChangesAsync();

        // 2. Create RFC referencing the already-saved suggested POI
        var rfc = CreateRfc(suggestedPoiId: sqlSuggestedPoi.Id);
        var sqlRfc = rfc.ToSqlRequestForChange();
        sqlRfc.SuggestedPoiId = sqlSuggestedPoi.Id;
        sqlRfc.SuggestedPoi = sqlSuggestedPoi; // Set navigation property if needed
        Context.RequestsForChange.Add(sqlRfc);
        await Context.SaveChangesAsync();

        var poi = CreatePoi();
        var request = new ApprovalRequest(rfc, poi);

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeTrue();

        var updatedRfc = Context.RequestsForChange.First(x => x.Id == rfc.Id);
        updatedRfc.ApprovalStatus.ShouldBe(ApprovalStatus.APPROVED);

        Context.SuggestedPointsOfInterest.FirstOrDefault(x => x.Id == suggestedPoi.Id).ShouldBeNull();

        var createdPoi = Context.PointsOfInterest.FirstOrDefault(x => x.Title == poi.Title);
        createdPoi.ShouldNotBeNull();
        createdPoi.StatusName.ShouldBe(PointOfInterestStatus.ACTIVE);
    }


    [Theory]
    [MemberData(nameof(PoiData))]
    public async Task CommitAsync_Should_Approve_Rfc_With_ExistingPoi(string title, string category, string status)
    {
        // Arrange
        var uow = new ApproveRfcUnitOfWork(Context);

        var poi = CreatePoi(title: title, category: category, status: status);
        Context.PointsOfInterest.Add(poi.ToSqlPointOfInterest());
        await Context.SaveChangesAsync();

        var rfc = CreateRfc(poiId: poi.Id);
        Context.RequestsForChange.Add(rfc.ToSqlRequestForChange());
        await Context.SaveChangesAsync();

        poi.Title = "Updated Title";
        var request = new ApprovalRequest(rfc, poi);

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeTrue();

        var updatedRfc = Context.RequestsForChange.First(x => x.Id == rfc.Id);
        updatedRfc.ApprovalStatus.ShouldBe(ApprovalStatus.APPROVED);

        var updatedPoi = Context.PointsOfInterest.First(x => x.Id == poi.Id);
        updatedPoi.Title.ShouldBe("Updated Title");
    }

    [Fact]
    public async Task CommitAsync_Should_Throw_If_Poi_Is_Null()
    {
        // Arrange
        var uow = new ApproveRfcUnitOfWork(Context);
        var rfc = CreateRfc();
        ApprovalRequest request = new(rfc, null!);

        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () => await uow.CommitAsync(request));
    }

    [Fact]
    public async Task CommitAsync_Should_Return_False_On_Exception()
    {
        // Arrange
        var uow = new ApproveRfcUnitOfWork(Context);

        // Create and save POI
        var poi = CreatePoi();
        Context.PointsOfInterest.Add(poi.ToSqlPointOfInterest());
        await Context.SaveChangesAsync();

        // Create and save RFC referencing the POI
        var rfc = CreateRfc(poiId: poi.Id);
        Context.RequestsForChange.Add(rfc.ToSqlRequestForChange());
        await Context.SaveChangesAsync();

        // Remove the "APPROVED" status to force an exception during approval
        var approved = await Context.ApprovalStatuses.FirstOrDefaultAsync(s => s.Status == ApprovalStatus.APPROVED);
        Context.ApprovalStatuses.Remove(approved!);
        await Context.SaveChangesAsync();

        var updatedPoi = poi;
        updatedPoi.Title = "ShouldNotMatter";
        var request = new ApprovalRequest(rfc, updatedPoi);

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeFalse();
    }
}