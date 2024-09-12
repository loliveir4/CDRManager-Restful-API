namespace CDRTests;

public static class TestHelpers
{
    public static CallDetailRecord CreateCallDetailRecord(
        string callerId = "441216000000",
        string recipient = "448004000000",
        DateTime? callDate = null,
        TimeSpan? endTime = null,
        int duration = 120,
        decimal cost = 0.1m,
        string reference = "DEFAULT",
        string currency = "GBP",
        CallType callType = CallType.Domestic)
    {
        return new CallDetailRecord
        {
            CallerId = callerId,
            Recipient = recipient,
            CallDate = callDate ?? DateTime.Now,
            EndTime = endTime ?? TimeSpan.Parse("14:21:33"),
            Duration = duration,
            Cost = cost,
            Reference = reference,
            Currency = currency,
            Type = callType
        };
    }
}
