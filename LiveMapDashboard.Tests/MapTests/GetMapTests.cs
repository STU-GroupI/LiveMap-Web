using LiveMap.Application.Map.Handlers;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;
using LiveMap.Domain.Models;
using LiveMapDashboard.Tests.PoiTests;
using Microsoft.OpenApi.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMapDashboard.Tests.MapTests;
public class GetMapTests
{
    [Theory]
    [MemberData(nameof(GetValidMap))]
    public async Task ReadSingle_WithValidId_ShouldReturnResponseWithMap(
        Map map,
        StubMapRepository repository)
    {
        // Arrange
        var request = new GetSingleRequest(map.Id);
        var handler = new GetSingleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.Map.ShouldBe(map);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000001")]
    public async Task ReadSingle_WithValidId_WhenMapDoesNotExist_ShouldReturnResponseWithoutMap(Guid nonExistentId)
    {
        // Arrange
        var request = new GetSingleRequest(nonExistentId);
        var handler = new GetSingleHandler(new StubMapRepository(new List<Map>()));

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.Map.ShouldBeNull();
    }

    [Theory]
    [MemberData(nameof(GetValidMaps))]
    public async Task ReadMultiple_WhenMapsExist_ShouldReturnExpectedMaps(
        StubMapRepository repository)
    {
        // Arrange
        var request = new GetMultipleRequest(null, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.Maps.Count().ShouldBe(repository.maps.Count());
    }

    [Theory]
    [MemberData(nameof(GetValidMapsWithSkipWhithoutTake))]
    public async Task ReadMultiple_WithValidSkip_WhenMapsExist_ShouldReturnExpectedMap(
        int skip,
        StubMapRepository repository)
    {
        // Arrange
        var expectedPois = repository.maps.Skip(skip).ToList();
        var request = new GetMultipleRequest(skip, null);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.Maps.ShouldBe(expectedPois);
    }

    [Theory]
    [MemberData(nameof(GetMapsWithSkipAndTake))]
    public async Task ReadMultiple_WithValidSkipAndTake_WhenMapsExist_ShouldReturnExpectedMaps(
        int skip,
        int take,
        StubMapRepository repository)
    {
        // Arrange
        var expectedPois = repository.maps.Skip(skip).Take(take).ToList();
        var request = new GetMultipleRequest(skip, take);
        var handler = new GetMultipleHandler(repository);

        // Act
        var actualResult = await handler.Handle(request);

        // Assert
        actualResult.Maps.ShouldBe(expectedPois);
    }

    public static IEnumerable<object[]> GetValidMap()
    {
        var repository = new StubMapRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { map, repository };
        }
    }
    public static IEnumerable<object[]> GetValidMaps()
    {
        var repository = new StubMapRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { repository };
        }
    }
    public static IEnumerable<object[]> GetValidMapsWithSkipWhithoutTake()
    {
        var repository = new StubMapRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { 5, repository };
            yield return new object[] { 10, repository };
            yield return new object[] { 0, repository };
        }
    }
    public static IEnumerable<object[]> GetMapsWithSkipAndTake()
    {
        var repository = new StubMapRepository(
            TestDataGenerator.GenerateMultipleMaps(3));

        foreach (var map in repository.maps)
        {
            yield return new object[] { 5, 10, repository };
            yield return new object[] { 10, 5, repository };
            yield return new object[] { 0, 10, repository };
        }
    }
}