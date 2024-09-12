using CDR.Core.Models;

namespace CDRManager.Services.Interfaces;

public interface IDbService
{
    Task AddCdrRecordsAsync(List<CallDetailRecord> cdrRecords);

    Task<CallDetailRecord?> SearchByReferenceAsync(string reference);

    Task<ResultPerMonth?> GetCallSummary(CallSumary callSumaryFilter);

    Task<List<CallDetailRecord>?> GetCdrsForCaller(CallerFilter callerSumaryFilter);

    Task<List<CallDetailRecord>?> GetMostExpensiveCalls(CallerFilter searchPerMonth, int N);
}
