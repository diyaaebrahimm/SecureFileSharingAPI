using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SecureFileSharingAPI
{
    /// <summary>
    /// Middleware that applies a simple in-memory rate limiting strategy.
    /// It identifies clients by an authorization token if present or by IP address otherwise.
    /// Requests exceeding the configured limits are rejected with HTTP 429.
    /// </summary>
    public class ContextAwareRateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ContextAwareRateLimiterMiddleware> _logger;

        // Stores timestamps of requests per client identifier
        private static readonly ConcurrentDictionary<string, List<DateTimeOffset>> _requestLog = new();

        // Example rule: allow up to 5 requests per minute per client
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);
        private const int MaxRequestsPerWindow = 5;

        public ContextAwareRateLimiterMiddleware(RequestDelegate next, ILogger<ContextAwareRateLimiterMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var identifier = GetClientIdentifier(context);
            var now = DateTimeOffset.UtcNow;

            var timestamps = _requestLog.GetOrAdd(identifier, _ => new List<DateTimeOffset>());

            lock (timestamps)
            {
                // Remove expired timestamps
                timestamps.RemoveAll(ts => now - ts > Window);

                // Check current request count
                if (timestamps.Count >= MaxRequestsPerWindow)
                {
                    _logger.LogWarning("Rate limit exceeded for {Identifier}", identifier);
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return;
                }

                timestamps.Add(now);
            }

            _logger.LogInformation("Request from {Identifier} allowed", identifier);
            await _next(context);
        }

        private static string GetClientIdentifier(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                return $"token:{authHeader}";
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            return $"ip:{ip}";
        }
    }
}

