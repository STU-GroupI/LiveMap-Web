using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Extensions;
using LiveMap.Persistence.UnitsOfWork.RequestForChange;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LiveMapDashboard.Tests.Perstistance.UnitOfWorkTests;

public class RejectRfcUnitOfWorkTests : TestBase
{
    private Guid _testMapId;

    protected override void SeedBaseData()
    {
        foreach (var status in new[] { ApprovalStatus.PENDING, ApprovalStatus.APPROVED, ApprovalStatus.REJECTED })
        {
            if (!Context.ApprovalStatuses.Any(s => s.Status == status))
                Context.ApprovalStatuses.Add(new ApprovalStatus { Status = status });
        }
        foreach (var status in new[] { PointOfInterestStatus.ACTIVE, PointOfInterestStatus.INACTIVE, PointOfInterestStatus.PENDING })
        {
            if (!Context.PoIStatusses.Any(s => s.Status == status))
                Context.PoIStatusses.Add(new PointOfInterestStatus { Status = status });
        }
        foreach (var cat in new[] { Category.STORE, Category.INFORMATION, Category.FIRSTAID_AND_MEDICAL, Category.TRASH_BIN, Category.PARKING, Category.ENTERTAINMENT, Category.EMPTY })
        {
            if (!Context.Categories.Any(c => c.CategoryName == cat))
                Context.Categories.Add(new Category { CategoryName = cat, IconName = cat + "Icon" });
        }
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

    [Fact]
    public async Task CommitAsync_Should_Reject_Rfc_With_SuggestedPoi()
    {
        // Arrange
        var uow = new RejectRfcUnitOfWork(Context);

        // 1. Create and save the suggested POI
        var suggestedPoi = CreateSuggestedPoi();
        var sqlSuggestedPoi = suggestedPoi.ToSqlSuggestedPointOfInterest();
        Context.SuggestedPointsOfInterest.Add(sqlSuggestedPoi);
        await Context.SaveChangesAsync();

        // 2. Create RFC referencing the already-saved suggested POI
        var rfc = CreateRfc(suggestedPoiId: sqlSuggestedPoi.Id);
        var sqlRfc = rfc.ToSqlRequestForChange();
        sqlRfc.SuggestedPoiId = sqlSuggestedPoi.Id;
        sqlRfc.SuggestedPoi = sqlSuggestedPoi;
        Context.RequestsForChange.Add(sqlRfc);
        await Context.SaveChangesAsync();

        var request = new RejectRfcRequest(rfc.Id);

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeTrue();

        var updatedRfc = Context.RequestsForChange.First(x => x.Id == rfc.Id);
        updatedRfc.ApprovalStatus.ShouldBe(ApprovalStatus.REJECTED);

        // Suggested POI should be deleted
        Context.SuggestedPointsOfInterest.FirstOrDefault(x => x.Id == suggestedPoi.Id).ShouldBeNull();
    }

    [Fact]
    public async Task CommitAsync_Should_Reject_Rfc_Without_SuggestedPoi()
    {
        // Arrange
        var uow = new RejectRfcUnitOfWork(Context);

        var rfc = CreateRfc();
        Context.RequestsForChange.Add(rfc.ToSqlRequestForChange());
        await Context.SaveChangesAsync();

        var request = new RejectRfcRequest(rfc.Id);

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeTrue();

        var updatedRfc = Context.RequestsForChange.First(x => x.Id == rfc.Id);
        updatedRfc.ApprovalStatus.ShouldBe(ApprovalStatus.REJECTED);
    }

    [Fact]
    public async Task CommitAsync_Should_Return_False_If_Rfc_Does_Not_Exist()
    {
        // Arrange
        var uow = new RejectRfcUnitOfWork(Context);

        var request = new RejectRfcRequest(Guid.NewGuid());

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task CommitAsync_Should_Return_False_On_Exception()
    {
        // Arrange
        var uow = new RejectRfcUnitOfWork(Context);

        // Create and save RFC
        var rfc = CreateRfc();
        Context.RequestsForChange.Add(rfc.ToSqlRequestForChange());
        await Context.SaveChangesAsync();

        // Remove the "REJECTED" status to force an exception during rejection
        var rejected = await Context.ApprovalStatuses.FirstOrDefaultAsync(s => s.Status == ApprovalStatus.REJECTED);
        Context.ApprovalStatuses.Remove(rejected!);
        await Context.SaveChangesAsync();

        var request = new RejectRfcRequest(rfc.Id);

        // Act
        var result = await uow.CommitAsync(request);

        // Assert
        result.ShouldBeFalse();
    }
}