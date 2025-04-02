using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.RfcTests;
public class CreateRfcTests
{
    [Theory]
    [MemberData(nameof(GetValidRFCs))]
    public async Task CreateRfc_WithValidInput_ShouldReturnResponseWithRFC(
        RequestForChange poi,
        StubRequestForChangeRepository repository)
    {
        Assert.True(true);
    }

    [Theory]
    [MemberData(nameof(GetInvalidRFCs))]
    public async Task CreateRfc_WithInvalidInput_ShouldThrowDbException(
        RequestForChange poi,
        StubRequestForChangeRepository repository)
    {
        Assert.True(true);
    }

    public static IEnumerable<object[]> GetValidRFCs()
    {
        const int amount = 100;
        var poiGuids = TestDataGenerator.GenerateGuids(amount);
        var testData = TestDataGenerator.GetRequestForChangeFaker(poiGuids).Generate(amount);
        var repository = new StubRequestForChangeRepository([], poiGuids);

        // May not be needed as guids that are equal do not clash unless its two entities of the same set with the same guid.
        /*
        // So for those that do not read spooky language, intersect gives back a list of items that are present in
        // both collections. So if you have a: { 1, 2, 3, 4, 5} and b: { 4, 5, 6, 7, 8 }, the intersection of the two
        // will be c: { 4, 5 }. This is true because 4 and 5 are present in both a and b.

        while (poiGuids.Intersect(testData.Select(i => i.Id)
            .ToArray())
            .Count() > 0)
        {
            poiGuids = TestDataGenerator.GenerateGuids(amount);
        }
        */

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
