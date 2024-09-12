namespace CDRTests.DbServiceTests;

//Arrange (Configuration), Act (Execution) and Assert (Validation)
public class GetCdrsPerCaller : TestBase
{

    [Fact]
    public async Task GetCdrsForCaller_ShouldReturnMatchingRecords_WhenRecordsExist()
    {
        // Arrange
        ClearDatabase();

        await AddCallRecordAsync();

        var callerFilter = CreateCallSummaryFilter(
             new DateTime(2023, 01, 15),
             new DateTime(2023, 01, 16));

        // Act
        var result = await _service.GetCdrsForCaller(callerFilter);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetCdrsForCaller_ShouldReturnNull_WhenDateRangeIsInvalid()
    {
        // Arrange
        ClearDatabase();

        var callerFilter = CreateCallSummaryFilter(
             new DateTime(2023, 02, 12),
             new DateTime(2023, 02, 11));

        // Act
        var result = await _service.GetCdrsForCaller(callerFilter);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCdrsForCaller_ShouldReturnEmptyList_WhenNoRecordsMatchFilter()
    {
        // Arrange
        ClearDatabase();

        var callerFilter = CreateCallSummaryFilter(
             new DateTime(2023, 02, 15),
             new DateTime(2023, 02, 16));


        // Act
        var result = await _service.GetCdrsForCaller(callerFilter);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);  
    }

    public static CallerFilter CreateCallSummaryFilter(DateTime inicialDate, DateTime finalDate, CallType callType = CallType.Domestic, string callerID = "441216000000")
    {
        return new CallerFilter
        {
            InicialDate = inicialDate,
            FinalDate = finalDate,
            CallType = callType,
            CallerId = callerID,
        };
    }

}
