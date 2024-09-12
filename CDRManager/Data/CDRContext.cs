using CDR.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CDRManager.Data;

public class CDRContext : DbContext
{
    public CDRContext(DbContextOptions<CDRContext> options)
       : base(options)
    {
    }

    public virtual DbSet<CallDetailRecord> CallDetailRecords { get; set; }
}
