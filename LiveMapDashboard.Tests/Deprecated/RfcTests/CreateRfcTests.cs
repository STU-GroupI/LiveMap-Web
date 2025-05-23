using LiveMap.Application.RequestForChange.Handlers;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.Deprecated.RfcTests;
public class CreateRfcTests
{
    [Theory]
    [MemberData(nameof(GetValidRFCs))]
    public async Task CreateRfc_WithValidInput_ShouldReturnResponseWithRFC(
        RequestForChange rfc,
        StubRequestForChangeRepository repository)
    {
        // Assert that a CreateSingleHandler returns the expected resonse, given a valid RFC (the rfc param)...
        // Create an expected result that is a value copy of the given RFC, and compare that value to the
        // RFC that comes from the actual CreateSingleResponse given by the CreateSingleHandler instance
        // you will create.
        var request = new CreateSingleRequest(rfc);
        var handler = new CreateSingleHandler(repository);

        var response = await handler.Handle(request);
        var data = response.Rfc;

        Assert.NotNull(response.Rfc);
        Assert.NotEqual(data.SubmittedOn, default);
        Assert.Equal(data.ApprovalStatus, ApprovalStatus.PENDING);
    }

    public static IEnumerable<object[]> GetValidRFCs()
    {
        const int amount = 100;
        var poiGuids = TestDataGenerator.GenerateGuids(amount);
        var testData = TestDataGenerator.GetRequestForChangeFaker(poiGuids).Generate(amount);
        var repository = new StubRequestForChangeRepository([], poiGuids);

        foreach (var rfc in testData)
        {
            yield return new object[] { rfc, repository };
        }
    }

    public static IEnumerable<object[]> GetInvalidRFCs()
    {
        const int amount = 100;
        var poiGuids = TestDataGenerator.GenerateGuids(amount);
        var testData = TestDataGenerator.GetInvalidRequestForChangeFaker(poiGuids).Generate(amount);

        var repository = new StubRequestForChangeRepository([], poiGuids);

        foreach (var rfc in testData)
        {
            yield return new object[] { rfc, repository };
        }
    }
}
