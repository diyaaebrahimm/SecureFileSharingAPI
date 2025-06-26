using Microsoft.EntityFrameworkCore;
using SecureFileSharingAPI;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ContextAwareRateLimiterMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapGet("/", () => "Secure File Sharing API");
app.MapGet("/api/test/ping", () => Results.Ok());

app.MapGet("/api/rate-limiter/stats", async (AppDbContext db) =>
{
    var today = DateTime.UtcNow.Date;

    var blockedClientsToday = await db.BlockEvents
        .CountAsync(b => b.BlockedUntil >= today);

    var mostTriggeredRule = await db.BlockEvents
        .GroupBy(b => b.RuleTriggered)
        .OrderByDescending(g => g.Count())
        .Select(g => g.Key)
        .FirstOrDefaultAsync();

    var topIps = await db.RequestLogs
        .Where(r => r.ClientId.StartsWith("ip:"))
        .GroupBy(r => r.ClientId)
        .Select(g => new IpActivityDto(
            g.Key.Substring(3),
            g.Count()))
        .OrderByDescending(x => x.RequestCount)
        .Take(5)
        .ToListAsync();

    var stats = new RateLimiterStats
    {
        BlockedClientsToday = blockedClientsToday,
        MostTriggeredRule = mostTriggeredRule,
        TopActiveIps = topIps
    };

    return Results.Json(stats);
});

app.Run();
