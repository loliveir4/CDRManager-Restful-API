using Microsoft.EntityFrameworkCore;

namespace CDRTests;

public abstract class TestBase : IDisposable
{
    protected readonly DbService _service;
    protected readonly CDRContext _context;

    public TestBase()
    {
        var options = new DbContextOptionsBuilder<CDRContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CDRContext(options);
        _service = new DbService(_context);
    }

    protected void ClearDatabase()
    {
        _context.CallDetailRecords.RemoveRange(_context.CallDetailRecords);
        _context.SaveChanges();
    }

    protected async Task AddCallRecordAsync()
    {
        // Primeiro registro
        var cdrRecord1 = TestHelpers.CreateCallDetailRecord(
            reference: "TEST123",
            callDate: new DateTime(2023, 01, 15),
            duration: 120,
            callType: CallType.Domestic
        );

        // Segundo registro
        var cdrRecord2 = TestHelpers.CreateCallDetailRecord(
            reference: "TEST124",
            callDate: new DateTime(2023, 01, 16),
            duration: 150,
            callType: CallType.Domestic
        );

        _context.CallDetailRecords.Add(cdrRecord1);
        _context.CallDetailRecords.Add(cdrRecord2);

        await _context.SaveChangesAsync();
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
