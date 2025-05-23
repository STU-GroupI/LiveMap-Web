using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Domain.Models;
using Shouldly;

namespace LiveMapDashboard.Tests.Deprecated.PoiTests;
public class DeletePoiTests
{
/*    [Theory]
    [MemberData(nameof(GetValidPointOfInterest))]
    public async Task DeleteSingle_WithValidId_WhenPoIExists_ShouldReturnResponseWithPoI(
    PointOfInterest poi,
    StubPointOfInterestRepository repository)
    {
        // Arrange
        var request = new DeleteSingleRequest(poi.Id);
        var handler = new DeleteSingleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointOfInterest.ShouldBe(poi);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    public async Task DeleteSingle_WithValidId_WhenPoIDoesNotExist_ShouldReturnResponseWithoutPoI(Guid nonExistentId)
    {
        // Arrange
        var request = new DeleteSingleRequest(nonExistentId);
        var handler = new DeleteSingleHandler(new StubPointOfInterestRepository(new List<Map>()));

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointOfInterest.ShouldBeNull();
    }

    public static IEnumerable<object[]> GetValidPointOfInterest()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var poi in repository.pois)
        {
            yield return new object[] { poi, repository };
        }
    }*/
}