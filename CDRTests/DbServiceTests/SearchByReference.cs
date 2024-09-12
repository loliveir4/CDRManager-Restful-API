namespace CDRTests.DbServiceTests;

//Arrange (Configuration), Act (Execution) and Assert (Validation)
public class SearchByReference : TestBase
{

    [Fact]
    public async Task SearchByReferenceAsync_ShouldReturnRecord_WhenRecordExists()
    {
        ClearDatabase();

        // Arrange
        await AddCallRecordAsync();

        // Act
        var result = await _service.SearchByReferenceAsync("TEST123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TEST123", result.Reference);
        Assert.Equal("441216000000", result.CallerId);
    }

    [Fact]
    public async Task SearchByReferenceAsync_ShouldReturnNull_WhenRecordDoesNotExist()
    {
        ClearDatabase();

        // Arrange
        await AddCallRecordAsync();

        // Act
        var result = await _service.SearchByReferenceAsync("NON_EXISTENT_REF");

        // Assert
        Assert.Null(result);
    }

}
