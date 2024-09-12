namespace CDRTests.DbServiceTests;

//Arrange (Configuration), Act (Execution) and Assert (Validation)
public class GetCallSummary : TestBase
{       
    [Fact]
    public async Task GetCallSummary_ShouldReturnCorrectSummary_WhenRecordsExist()
    {
        ClearDatabase();

        // Arrange
        await AddCallRecordAsync();

        var callSummaryFilter = CreateCallSummaryFilter(
       new DateTime(2023, 01, 15),
       new DateTime(2023, 01, 16)
         );

        // Act
        var result = await _service.GetCallSummary(callSummaryFilter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalCalls);
        Assert.Equal(270, result.TotalDuration);
    }

    [Fact]
    public async Task GetCallSummary_ShouldReturnNull_WhenDateRangeIsInvalid()
    {
        ClearDatabase();

        // Arrange
        var callSummaryFilter = CreateCallSummaryFilter(
         new DateTime(2023, 09, 12),
         new DateTime(2023, 10, 16)
         );
        // Act
        var result = await _service.GetCallSummary(callSummaryFilter);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCallSummary_ShouldReturnNull_WhenNoRecordsExist()
    {
        ClearDatabase();

        // Arrange
        var callSummaryFilter = CreateCallSummaryFilter(
         new DateTime(2023, 02, 15),
         new DateTime(2023, 02, 16)
       );

        // Act
        var result = await _service.GetCallSummary(callSummaryFilter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result?.TotalCalls);
        Assert.Equal(0, result?.TotalDuration);
    }

    private static CallSumary CreateCallSummaryFilter(DateTime inicialDate, DateTime finalDate, CallType callType = CallType.Domestic)
    {
        return new CallSumary
        {
            InicialDate = inicialDate,
            FinalDate = finalDate,
            CallType = callType
        };
    }
}