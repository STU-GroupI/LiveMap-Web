using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LiveMapDashboard.Tests.Perstistance.RepositoryTests;

public class RequestForChangeRepositoryTests : TestBase
{
    private static readonly string[] RequiredStatuses = new[]
    {
        ApprovalStatus.PENDING,
        ApprovalStatus.APPROVED,
        ApprovalStatus.REJECTED
    };

    protected override void SeedBaseData()
    {
        // Ensure ApprovalStatus table has required statuses
        foreach (var status in RequiredStatuses)
        {
            if (!Context.ApprovalStatuses.Any(s => s.Status == status))
            {
                Context.ApprovalStatuses.Add(new ApprovalStatus { Status = status });
            }
        }
        Context.SaveChanges();
    }

    private static RequestForChange CreateRfc(Guid? id = null, string? message = null)
        => new RequestForChange
        {
            Id = id ?? Guid.NewGuid(),
            Message = message ?? $"TestMessage_{Guid.NewGuid()}",
            ApprovalStatus = ApprovalStatus.PENDING,
            SubmittedOn = DateTime.UtcNow
        };

    [Fact]
    public async Task CreateAsync_Should_Add_Rfc_And_Return_It()
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc = CreateRfc();
        var result = await repo.CreateAsync(rfc);

        result.ShouldNotBeNull();
        result.Message.ShouldBe(rfc.Message);
        result.ApprovalStatus.ShouldBe(ApprovalStatus.PENDING);
    }

    [Fact]
    public async Task GetSingle_Should_Return_Rfc_If_Exists()
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc = await repo.CreateAsync(CreateRfc());
        var result = await repo.GetSingle(rfc.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(rfc.Id);
    }

    [Fact]
    public async Task GetSingle_Should_Return_Null_If_Not_Exists()
    {
        var repo = new RequestForChangeRepository(Context);

        var result = await repo.GetSingle(Guid.NewGuid());
        result.ShouldBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_Rfc()
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc = await repo.CreateAsync(CreateRfc());
        rfc.Message = "Updated message";
        rfc.ApprovalStatus = ApprovalStatus.APPROVED;

        var result = await repo.UpdateAsync(rfc);

        result.ShouldNotBeNull();
        result.Message.ShouldBe("Updated message");
        result.ApprovalStatus.ShouldBe(ApprovalStatus.APPROVED);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_If_Rfc_Does_Not_Exist()
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc = CreateRfc();
        var result = await repo.UpdateAsync(rfc);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetMultiple_Should_Return_Paginated_Rfcs()
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc1 = await repo.CreateAsync(CreateRfc(message: "A"));
        var rfc2 = await repo.CreateAsync(CreateRfc(message: "B"));
        var rfc3 = await repo.CreateAsync(CreateRfc(message: "C"));

        // Use a dummy parkId (not used in current implementation)
        var result = await repo.GetMultiple(Guid.Empty, skip: 1, take: 1, ascending: null, IsPending: null);

        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(1);
        result.TotalCount.ShouldBe(3);
        result.Items[0].Message.ShouldBe("B");
    }
}