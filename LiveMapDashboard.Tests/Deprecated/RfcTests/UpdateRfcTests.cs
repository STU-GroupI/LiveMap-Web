using LiveMap.Application.RequestForChange.Handlers;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.Deprecated.RfcTests;

public class UpdateRfcTests
{
    [Theory]
    [MemberData(nameof(GetValidRfcs))]
    public async Task UpdateRfc_WithValidData_ShouldReturnUpdatedRfc(
        RequestForChange rfc,
        StubRequestForChangeRepository repository)
    {
        // Arrange
        repository.RequestsForChange.Add(rfc);

        var updatedRfc = new RequestForChange()
        {
            Id = rfc.Id,
            ApprovedOn = DateTime.UtcNow,
            ApprovalStatus = ApprovalStatus.APPROVED,
            SubmittedOn = rfc.SubmittedOn
        };
        
        var request = new UpdateSingleRequest(updatedRfc);
        var handler = new UpdateSingleHandler(repository);
        
        // Act
        var response = await handler.Handle(request);
        var result = response.Rfc;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(ApprovalStatus.APPROVED, result.ApprovalStatus);
    }

    public static IEnumerable<object[]> GetValidRfcs()
    {
        const int amount = 100;
        var poiGuids = TestDataGenerator.GenerateGuids(amount);
        var testData = TestDataGenerator.GetRequestForChangeFaker(poiGuids).Generate(amount);
        var repository = new StubRequestForChangeRepository(new List<RequestForChange>(), poiGuids);

        foreach (var rfc in testData)
        {
            yield return new object[] { rfc, repository };
        }
        
    }
}