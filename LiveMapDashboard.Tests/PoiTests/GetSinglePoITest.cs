using LiveMap.Application.PointOfInterest.Handlers;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMapDashboard.Tests.PoiTests;
public class GetSinglePoITest
{
    public readonly StubPointOfInterestRepository repository;

    public GetSinglePoITest()
    {
        var repo = new StubPointOfInterestRepository(
            TestDataGenerator.GenerateMultiplePointsOfInterest(100));

        repository = repo;
    }

    [Fact]
    public async Task Given_ValidGuid_When_PoIExists_Then_ResponseWithPoI()
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

    [Fact]
    public async Task Given_ValidGuid_When_PoINotExists_Then_ResponseWithoutPoI()
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
}