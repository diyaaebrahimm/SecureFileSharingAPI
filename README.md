# SecureFileSharingAPI

This repository demonstrates a simple ASP.NET Core middleware called `ContextAwareRateLimiterMiddleware` used for rate limiting based on client context (IP address or Authorization token). The middleware tracks request timestamps in memory and blocks clients exceeding a fixed limit.

## Middleware Usage
Add the middleware in your `Startup` or minimal API configuration:
```csharp
app.UseMiddleware<ContextAwareRateLimiterMiddleware>();
```

The middleware will allow up to five requests per minute for each client.

