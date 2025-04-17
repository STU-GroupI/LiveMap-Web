using Bogus;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.RfcTests;
public static class TestDataGenerator
{
    public static readonly DateTime START_DATE = new DateTime(year: 2025, month: 4, day: 1, hour: 14, minute: 08, second: 0);
    public static readonly List<ApprovalStatus> APPROVAL_STATUSSES = new List<ApprovalStatus>([
        new() { Status = ApprovalStatus.REJECTED },
        new() { Status = ApprovalStatus.PENDING },
        new() { Status = ApprovalStatus.APPROVED }
    ]);

    public static Faker<RequestForChange> GetRequestForChangeFaker(
        List<ApprovalStatus> approvalStatuses,
        DateTime startDateTime,
        List<Guid> poiGuids)
    {
        var trackedPoiGuids = new List<Guid>(poiGuids);

        var pickRandomGuid = (Faker f) =>
        {
            var guid = f.PickRandom(trackedPoiGuids);
            trackedPoiGuids.Remove(guid);
            return guid;
        };

        return new Faker<RequestForChange>()
            .RuleFor(e => e.Id, f => f.Random.Guid())
            .RuleFor(e => e.PoiId, f => pickRandomGuid(f))
            .RuleFor(e => e.SubmittedOn, f => f.Date.Future(yearsToGoForward: 1, startDateTime))
            .RuleFor(e => e.ApprovalStatus, ApprovalStatus.PENDING)
            .RuleFor(e => e.Message, f => f.Lorem.Paragraphs(3));
    }

    public static Faker<RequestForChange> GetRequestForChangeFaker(
        List<Guid> poiGuids)
    {
        return GetRequestForChangeFaker(APPROVAL_STATUSSES, START_DATE, poiGuids);
    }

    public static Faker<RequestForChange> GetInvalidRequestForChangeFaker(
        List<ApprovalStatus> approvalStatuses,
        DateTime startDateTime,
        List<Guid> poiGuids)
    {
        var trackedPoiGuids = new List<Guid>(poiGuids);
        var random = new Random();
        var invalidGuid = false;

        var pickRandomGuid = (Faker f) =>
        {
            if (random.Next(0, 12) % 3 == 0)
            {
                invalidGuid = true;
                return f.Random.Guid();
            }

            var guid = f.PickRandom(trackedPoiGuids);
            trackedPoiGuids.Remove(guid);
            return guid;
        };

        var generateMessage = (Faker f) =>
        {
            if (invalidGuid)
            {
                invalidGuid = !invalidGuid;
                return f.Lorem.Paragraphs(2);
            }

            return null;
        };

        return new Faker<RequestForChange>()
            .RuleFor(e => e.Id, f => f.Random.Guid())
            .RuleFor(e => e.PoiId, f => pickRandomGuid(f))
            .RuleFor(e => e.SubmittedOn, f => f.Date.Future(yearsToGoForward: 1, startDateTime))
            .RuleFor(e => e.ApprovalStatus, ApprovalStatus.PENDING)
            .RuleFor(e => e.Message, f => generateMessage(f));
    }

    public static Faker<RequestForChange> GetInvalidRequestForChangeFaker(
        List<Guid> poiGuids)
    {
        return GetInvalidRequestForChangeFaker(APPROVAL_STATUSSES, START_DATE, poiGuids);
    }

    public static List<Guid> GenerateGuids(int amount)
    {
        List<Guid> guids = new List<Guid>();
        var faker = new Faker();

        for (int i = 0; i < amount; i++)
        {
            guids.Add(faker.Random.Guid());
        }

        return guids;
    }
}
