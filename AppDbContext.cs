using Microsoft.EntityFrameworkCore;

namespace SecureFileSharingAPI;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RequestLog> RequestLogs => Set<RequestLog>();
    public DbSet<BlockEvent> BlockEvents => Set<BlockEvent>();
    public DbSet<ClientStatus> ClientStatuses => Set<ClientStatus>();
}
