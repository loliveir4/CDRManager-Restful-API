namespace CDRTests.DbServiceTests;

//Arrange (Configuration), Act (Execution) and Assert (Validation)
public class GetMostExpensiveCallsT : TestBase
{

    [Fact]
    public async Task GetMostExpensiveCalls_ShouldReturnTopNMostExpensiveCalls_WhenRecordsExist()
    {
        // Arrange
        ClearDatabase();

        await AddCallRecordAsync();

        var callerFilter = GetCdrsPerCaller.CreateCallSummaryFilter(
             new DateTime(2023, 01, 15),
             new DateTime(2023, 01, 16));

        // Act
        var result = await _service.GetMostExpensiveCalls(callerFilter, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(0.1m, result[0].Cost);
        Assert.Equal(0.1m, result[1].Cost);
    }

    [Fact]
    public async Task GetMostExpensiveCalls_ShouldReturnNull_WhenDateRangeIsInvalid()
    {
        // Arrange
        ClearDatabase();

        var callerFilter = GetCdrsPerCaller.CreateCallSummaryFilter(
              new DateTime(2023, 01, 15),
              new DateTime(2023, 01, 14));

        // Act
        var result = await _service.GetMostExpensiveCalls(callerFilter, 3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public async Task GetMostExpensiveCalls_ShouldReturnEmptyList_WhenNoCallsMatchFilter()
    {
        // Arrange
        ClearDatabase();

        var callerFilter = GetCdrsPerCaller.CreateCallSummaryFilter(
                 new DateTime(2023, 01, 15),
                 new DateTime(2023, 01, 16));

        // Act
        var result = await _service.GetMostExpensiveCalls(callerFilter, 3);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }


}
