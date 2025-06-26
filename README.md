# SecureFileSharingAPI

This repository demonstrates a simple ASP.NET Core middleware called `ContextAwareRateLimiterMiddleware` used for rate limiting based on client context (IP address or Authorization token). The middleware tracks request timestamps in memory and blocks clients exceeding a fixed limit.

## Middleware Usage
Add the middleware in your `Startup` or minimal API configuration:
```csharp
app.UseMiddleware<ContextAwareRateLimiterMiddleware>();
```

The middleware will allow up to five requests per minute for each client.


## Rate Limit Rules

Custom rules can be implemented using the `IRateLimitRule` interface. The provided `LoginBurstRule`
blocks clients making more than five requests to the `/login` endpoint within ten seconds.

## Database Setup

The sample includes an `AppDbContext` configured for SQLite. Run EF Core migrations to create the schema:
```bash
# add the initial migration
 dotnet ef migrations add InitialCreate

# update the database file
 dotnet ef database update
```
The connection string defaults to `Data Source=app.db` and can be overridden in `appsettings.json`.

## Testing Rate Limiting

1. Run the API:
   ```bash
   dotnet run
   ```
2. Open the Swagger UI at `http://localhost:5000/swagger` to explore endpoints. The sample exposes `/api/test/ping` which simply returns `200 OK`.
3. Use a tool like Postman to send rapid requests to `/api/test/ping`.
4. After five requests within a minute, the middleware responds with HTTP `429 Too Many Requests`, demonstrating rate limiting.
