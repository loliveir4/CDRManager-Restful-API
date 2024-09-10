using CDRManager.Migrations;
using Microsoft.EntityFrameworkCore;

namespace CDRManager.Services;

public class DbService : IDbService
{
    private readonly CDRContext _context;

    public DbService(CDRContext context) => _context = context;

    /// <summary>
    /// Reads a list of CDR records from a source (like a CSV file) and saves the data to SQL Server.
    /// </summary>
    public async Task AddCdrRecordsAsync(List<CallDetailRecord> cdrRecords)
    {
        if (cdrRecords == null || cdrRecords.Count == 0)
            throw new ArgumentException("The CDR list is empty or null.");

        await _context.CallDetailRecords.AddRangeAsync(cdrRecords);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Searches for a specific call record based on a unique reference string.
    /// </summary>
    public async Task<CallDetailRecord?> SearchByReferenceAsync(string reference)
    {
        var cdrFound = await _context.CallDetailRecords
                                     .FirstOrDefaultAsync(d => d.Reference == reference);

        return cdrFound;
    }

    /// <summary>
    /// Retrieves a summary of calls within a specified month, including the total call count and duration.
    /// </summary>
    public async Task<ResultPerMonth?> GetCallSummary(CallSumary callSumaryFilter)
    {
        if (!ValidateDateRange(callSumaryFilter.InicialDate, callSumaryFilter.FinalDate))
        {
            return null;
        }

        var query = _context.CallDetailRecords
                            .Where(d => d.CallDate >= callSumaryFilter.InicialDate &&
                                        d.CallDate <= callSumaryFilter.FinalDate);

        if (callSumaryFilter.CallType != 0)
        {
            query = query.Where(d => d.Type == callSumaryFilter.CallType);
        }

        var totalCalls = await query.CountAsync();
        var totalDuration = await query.SumAsync(d => d.Duration);

        return new ResultPerMonth
        {
            TotalCalls = totalCalls,
            TotalDuration = totalDuration
        };
    }

    /// <summary>
    /// Retrieves a list of CDRs for a specific caller ID within a specified month, optionally filtered by call type.
    /// </summary>
    public async Task<List<CallDetailRecord>?> GetCdrsForCaller(CallerFilter callerSumaryFilter)
    {
        if (!ValidateDateRange(callerSumaryFilter.InicialDate, callerSumaryFilter.FinalDate))
        {
            return null;
        }

            var query = _context.CallDetailRecords
                            .Where(d => d.CallerId == callerSumaryFilter.CallerId);

        query = ApplyFilters(query, callerSumaryFilter.InicialDate, callerSumaryFilter.FinalDate, callerSumaryFilter.CallType);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves the N most expensive calls (in GBP) for a specific caller within a specified month, optionally filtered by call type.
    /// </summary>
    public async Task<List<CallDetailRecord>?> GetMostExpensiveCalls(CallerFilter callerSumaryFilter, int N)
    {
        if (!ValidateDateRange(callerSumaryFilter.InicialDate, callerSumaryFilter.FinalDate))
        {
            return null;
        }
        var query = _context.CallDetailRecords
                            .Where(d => d.CallerId == callerSumaryFilter.CallerId);

        query = ApplyFilters(query, callerSumaryFilter.InicialDate, callerSumaryFilter.FinalDate, callerSumaryFilter.CallType);

        var mostExpensiveCalls = await query
                              .Where(d => d.Currency == "GBP")
                              .OrderByDescending(d => d.Cost)
                              .Take(N)
                              .ToListAsync();


        return mostExpensiveCalls;
    }

    /// <summary>
    /// Applies filters to the CDR query, based on date range and call type.
    /// </summary>
    private static IQueryable<CallDetailRecord> ApplyFilters(IQueryable<CallDetailRecord> query, DateTime initialDate, DateTime finalDate, CallType callType)
    {
        query = query.Where(d => d.CallDate >= initialDate && d.CallDate <= finalDate);

        if (callType != 0)
        {
            query = query.Where(d => d.Type == callType);
        }

        return query;
    }

    /// <summary>
    /// Validates that the provided date range does not exceed 1 month.
    /// </summary>
    private static bool ValidateDateRange(DateTime initialDate, DateTime finalDate)
    {
        return (finalDate - initialDate).TotalDays <= 31;
    }
}
