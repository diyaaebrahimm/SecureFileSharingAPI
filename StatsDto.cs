using System.Collections.Generic;

namespace SecureFileSharingAPI;

public record IpActivityDto(string Ip, int RequestCount);

public class RateLimiterStats
{
    public int BlockedClientsToday { get; set; }
    public string? MostTriggeredRule { get; set; }
    public List<IpActivityDto> TopActiveIps { get; set; } = new();
}
