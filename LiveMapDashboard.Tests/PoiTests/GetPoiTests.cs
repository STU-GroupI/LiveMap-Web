using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Domain.Models;
using Microsoft.OpenApi.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMapDashboard.Tests.PoiTests;
public class GetPoiTests
{
    [Theory]
    [MemberData(nameof(GetValidPointOfInterest))]
    public async Task ReadSingle_WithValidId_WhenPoIExists_ShouldReturnResponseWithPoI(
    PointOfInterest poi,
    StubPointOfInterestRepository repository)
    {
        // Arrange
        var request = new GetSingleRequest(poi.Id);
        var handler = new GetSingleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointOfInterest.ShouldBe(poi);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    public async Task ReadSingle_WithValidId_WhenPoIDoesNotExist_ShouldReturnResponseWithoutPoI(Guid nonExistentId)
    {
        // Arrange
        var request = new GetSingleRequest(nonExistentId);
        var handler = new GetSingleHandler(new StubPointOfInterestRepository(new List<Map>()));

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointOfInterest.ShouldBeNull();
    }

    [Theory]
    [MemberData(nameof(GetValidMap))]
    public async Task ReadMultiple_WithValidMapId_WhenMapExistsAndHasPois_ShouldReturnExpectedPoisForMap(
        Map map,
        StubPointOfInterestRepository repository)
    {
        // Arrange
        var request = new GetMultipleRequest(map.Id, null, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointsOfInterests.Count().ShouldBe(map.PointOfInterests.Count());
    }

    [Theory]
    [MemberData(nameof(GetValidMapIdsWithSkipWithoutTake))]
    public async Task ReadMultiple_WithValidMapId_AndValidSkip_WhenMapExistsAndHasPois_ShouldReturnExpectedPoisFromMap(
        Map map,
        int skip,
        StubPointOfInterestRepository repository)
    {
        // Arrange
        var expectedPois = map.PointOfInterests.Skip(skip).ToList();
        var request = new GetMultipleRequest(map.Id, skip, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointsOfInterests.ShouldBe(expectedPois);
    }

    [Theory]
    [MemberData(nameof(GetValidMapIdsWithSkipAndTake))]
    public async Task ReadMultiple_WithValidMapId_AndValidSkipAndTake_WhenMapExistsAndHasPois_ShouldReturnExpectedPoisForMap(
        Map map,
        int skip,
        int take,
        StubPointOfInterestRepository repository)
    {
        // Arrange
        var expectedPois = map.PointOfInterests.Skip(skip).Take(take).ToList();
        var request = new GetMultipleRequest(map.Id, skip, take);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointsOfInterests.ShouldBe(expectedPois);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000002")] // Non-existing Map ID
    public async Task ReadMultiple_WithInvalidMapId_ShouldReturnEmptyResponse(Guid invalidMapId)
    {
        // Arrange
        var request = new GetMultipleRequest(invalidMapId, null, null);
        var handler = new GetMultipleHandler(new StubPointOfInterestRepository(new List<Map>()));

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointsOfInterests.ShouldBeEmpty();
    }

    [Theory]
    [MemberData(nameof(GetValidMapsWithoutPois))]
    public async Task ReadMultiple_WithValidMapId_WhenMapHasNoPois_ShouldReturnEmptyResponse(
        Map map,
        StubPointOfInterestRepository repository)
    {
        // Arrange
        var request = new GetMultipleRequest(map.Id, null, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.PointsOfInterests.ShouldBeEmpty();
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
    public static IEnumerable<object[]> GetValidMap()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { map, repository };
        }
    }
    public static IEnumerable<object[]> GetValidMapsWithoutPois()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3, false));

        foreach (var map in repository.maps)
        {
            yield return new object[] { map, repository };
        }
    }
    public static IEnumerable<object[]> GetValidMapIdsWithSkipWithoutTake()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { map, 5, repository };
            yield return new object[] { map, 10, repository };
            yield return new object[] { map, 0, repository };
        }
    }
    public static IEnumerable<object[]> GetValidMapIdsWithSkipAndTake()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { map, 5, 10, repository };
            yield return new object[] { map, 10, 5, repository };
            yield return new object[] { map, 0, 10, repository };
        }
    }
}