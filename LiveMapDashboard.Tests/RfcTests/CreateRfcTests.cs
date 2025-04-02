using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.RfcTests;
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
        Assert.True(true);
    }

    [Theory]
    [MemberData(nameof(GetInvalidRFCs))]
    public async Task CreateRfc_WithInvalidInput_ShouldThrowDbException(
        RequestForChange rfc,
        StubRequestForChangeRepository repository)
    {
        // Assert that a CreateSingleHandler throws a DbException when given an invalid request with the given RFC.
        // Consult the previous unit test for a how to do the initial setup.

        // Be mindfull, it may be the case that you get a TaskCanceledException. That would be valid to as long as your check
        // the inner exception and make sure its a DbException somewhere down the line. You may consult the following code:
        
        // var ex = new TaskCanceledException("This task was canceled", new ArgumentException());
        // DbException dbEx = ex.InnerException;
        // Assert.IsType<DbException>(dbEx)

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
