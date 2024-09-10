using CDRLibrary.Models;
using CDRManager.Data;
using CDRManager.Services;
using Microsoft.EntityFrameworkCore;

namespace CDRTests;

public abstract class TestBase : IDisposable
{
    protected readonly DbService _service;
    protected readonly CDRContext _context;

    public TestBase()
    {
        // Configurando o DbContext para usar um banco de dados InMemory
        var options = new DbContextOptionsBuilder<CDRContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())  // Nome único para cada teste
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
        var cdrRecord = TestHelpers.CreateCallDetailRecord(reference: "TEST123", callDate: new DateTime(2023, 01, 15), duration: 120, callType: CallType.Domestic);
        _context.CallDetailRecords.Add(cdrRecord);
        await _context.SaveChangesAsync();
    }

    // Método Dispose para liberar os recursos do contexto após os testes
    public void Dispose()
    {
        _context.Dispose();
    }
}
