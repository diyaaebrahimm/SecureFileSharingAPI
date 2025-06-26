using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SecureFileSharingAPI
{
    /// <summary>
    /// Blocks a client if more than a configured number of requests
    /// are made to the /login endpoint within a short window.
    /// </summary>
    public class LoginBurstRule : IRateLimitRule
    {
        private readonly TimeSpan _window = TimeSpan.FromSeconds(10);
        private readonly int _maxRequests;

        public LoginBurstRule(int maxRequests = 5)
        {
            _maxRequests = maxRequests;
        }

        public ThrottleDecision Evaluate(ClientProfile profile, HttpRequest request)
        {
            if (!string.Equals(request.Path, "/login", StringComparison.OrdinalIgnoreCase))
            {
                return ThrottleDecision.Allow;
            }

            var now = DateTimeOffset.UtcNow;
            profile.LogRequest("/login", now);
            var timestamps = profile.GetRequests("/login");

            // Remove outdated entries
            timestamps.RemoveAll(ts => now - ts > _window);

            var recentCount = timestamps.Count(ts => now - ts <= _window);
            if (recentCount > _maxRequests)
            {
                return ThrottleDecision.Block;
            }

            return ThrottleDecision.Allow;
        }
    }
}
