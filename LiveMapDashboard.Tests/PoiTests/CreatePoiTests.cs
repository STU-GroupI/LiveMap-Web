using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Domain.Models;
using Shouldly;

namespace LiveMapDashboard.Tests.PoiTests;

public class CreatePoiTests
{
    [Theory]
    [MemberData(nameof(GetValidPointOfInterest))]
    public async Task CreateSingle_WithValidInput_ShouldReturnResponseWithPoI(
    PointOfInterest poi,
    StubPointOfInterestRepository repository)
    {
        // Arrange
        var request = new CreateSingleRequest(poi);
        var handler = new CreateSingleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.Poi.ShouldBe(poi);
    }

    public static IEnumerable<object[]> GetValidPointOfInterest()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var poi in repository.pois)
        {
            yield return new object[] { poi, repository };
        }
    }
}