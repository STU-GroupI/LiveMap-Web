namespace LiveMapDashboard.Tests;

public class DummyTest
{
    [Fact]
    public void IndexTest()
    {
        // Arrange
        int index = 0;

        // Act
        index += 2;

        // Assert
        Assert.Equal(2, index);
    }
}
