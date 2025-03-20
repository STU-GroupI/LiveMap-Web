using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Domain.Models;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMapDashboard.Tests.PoiTests;
public class GetPoiTests
{
    public readonly StubPointOfInterestRepository repository;

    public GetPoiTests()
    {
        var repo = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        repository = repo;
    }

    [Theory]
    [MemberData(nameof(GetValidPointOfInterestIds))]
    public async Task Doing_ReadSingle_Given_ValidId_When_PoIExists_ResultsTo_ResponseWithPoI()
    {
        // Arrange
        var expectedPoi = repository.points[0];
        var expectedResponse = new GetSingleResponse(expectedPoi);

        var guid = expectedPoi.Id;
        var request = new GetSingleRequest(guid);
        var handler = new GetSingleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Same(actualResult.PointOfInterest, expectedPoi);
    }

    public static IEnumerable<object[]> GetValidPointOfInterestIds()
    {
        var repository = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var poi in repository.points)
        {
            yield return new object[] { poi.Id };
        }
    }

    [Fact]
    public async Task Doing_ReadSingle_Given_ValidId_When_PoINotExists_ResultsTo_ResponseWithoutPoI()
    {
        // Arrange
        PointOfInterest? expectedPoi = null;
        var expectedResponse = new GetSingleResponse(expectedPoi);

        var guid = Guid.NewGuid();
        var request = new GetSingleRequest(guid);
        var handler = new GetSingleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Null(actualResult.PointOfInterest);
    }

    [Fact]
    public async Task Doing_ReadMultiple_Given_ValidMapId_When_MapExists_And_MapHasPois_ResultsTo_ExpectedPoisForMap()
    {
        // Arrange
        var map = repository.maps[0];
        var expectedResponse = new GetMultipleResponse(map.PointOfInterests);

        var guid = map.Id;
        var request = new GetMultipleRequest(guid, null, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Equal(expectedResponse.PointsOfInterests.Count(), actualResult.PointsOfInterests.Count());
    }

    [Fact]
    public async Task Doing_ReadMultiple_Given_ValidMapId_With_ValidSkip_When_MapExists_And_MapHasPois_ResultsTo_ExpectedPoisFromMap()
    {
        // Arrange
        var map = repository.maps[0];
        int skip = 10;
        var expectedResponse = new GetMultipleResponse(map.PointOfInterests
            .Skip(skip)
            .ToList());

        var guid = map.Id;
        var request = new GetMultipleRequest(guid, skip, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Equal(expectedResponse.PointsOfInterests.Count(), actualResult.PointsOfInterests.Count());
    }

    [Fact]
    public async Task Doing_ReadMultiple_Given_ValidMapId_With_ValidSkip_And_ValidTake_When_MapExists_And_MapHasPois_ResultsTo_ExpectedPoisForMap()
    {
        // Arrange
        var map = repository.maps[0];
        int skip = 10;
        int take = 10;
        var expectedResponse = new GetMultipleResponse(map.PointOfInterests
            .Skip(skip)
            .Take(take)
            .ToList());

        var guid = map.Id;
        var request = new GetMultipleRequest(guid, skip, take);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Equal(expectedResponse.PointsOfInterests.Count(), actualResult.PointsOfInterests.Count());
    }

    [Fact]
    public async Task Doing_ReadMultiple_Given_InvalidMapId_ResultsTo_EmptyResponse()
    {
        // Arrange
        var expectedResponse = new GetMultipleResponse([]);

        var guid = Guid.NewGuid();
        var request = new GetMultipleRequest(guid, null, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Equal(expectedResponse.PointsOfInterests.Count(), actualResult.PointsOfInterests.Count());
    }

    [Fact]
    public async Task Doing_ReadMultiple_Given_ValidMapId_When_MapHasNoPois_ResultsTo_EmptyResponse()
    {
        // Arrange
        var expectedResponse = new GetMultipleResponse([]);
        var guid = Guid.NewGuid();
        var request = new GetMultipleRequest(guid, null, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        Assert.Equal(expectedResponse.PointsOfInterests.Count(), actualResult.PointsOfInterests.Count());
    }
}