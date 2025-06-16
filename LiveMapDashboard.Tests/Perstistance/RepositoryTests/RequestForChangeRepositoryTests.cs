using Bogus;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using Shouldly;

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

    public static IEnumerable<object[]> CreateRfcData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 3; i++)
            {
                yield return new object[]
                {
                    faker.Lorem.Sentence()
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(CreateRfcData))]
    public async Task CreateAsync_Should_Add_Rfc_And_Return_It(string message)
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc = CreateRfc(message: message);
        var result = await repo.CreateAsync(rfc);

        result.ShouldNotBeNull();
        result.Message.ShouldBe(rfc.Message);
        result.ApprovalStatus.ShouldBe(ApprovalStatus.PENDING);
    }

    public static IEnumerable<object[]> GetSingleRfcData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                yield return new object[]
                {
                    faker.Lorem.Sentence()
                };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetSingleRfcData))]
    public async Task GetSingle_Should_Return_Rfc_If_Exists(string message)
    {
        var repo = new RequestForChangeRepository(Context);

        var rfc = await repo.CreateAsync(CreateRfc(message: message));
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

    public static IEnumerable<object[]> GetMultipleRfcData
    {
        get
        {
            var faker = new Faker();
            for (int i = 0; i < 2; i++)
            {
                var messages = new[]
                {
                    faker.Lorem.Word(),
                    faker.Lorem.Word(),
                    faker.Lorem.Word()
                };
                yield return new object[] { 1, 1, messages, messages[1] };
                yield return new object[] { 0, 2, messages, messages[0] };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetMultipleRfcData))]
    public async Task GetMultiple_Should_Return_Paginated_Rfcs(int skip, int take, string[] messages, string expectedMessage)
    {
        var repo = new RequestForChangeRepository(Context);

        foreach (var message in messages)
        {
            await repo.CreateAsync(CreateRfc(message: message));
        }

        // Use a dummy parkId (not used in current implementation)
        var result = await repo.GetMultiple(Guid.Empty, skip: skip, take: take, ascending: null, IsPending: null);

        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(take);
        result.TotalCount.ShouldBe(messages.Length);
        result.Items[0].Message.ShouldBe(expectedMessage);
    }
}