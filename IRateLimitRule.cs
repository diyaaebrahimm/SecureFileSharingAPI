using Microsoft.AspNetCore.Http;

namespace SecureFileSharingAPI
{
    /// <summary>
    /// Represents a rule that evaluates whether a request should be allowed,
    /// blocked or logged based on the client's profile and request details.
    /// </summary>
    public interface IRateLimitRule
    {
        ThrottleDecision Evaluate(ClientProfile profile, HttpRequest request);
    }
}
