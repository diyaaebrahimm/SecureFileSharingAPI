using System;
using System.ComponentModel.DataAnnotations;

namespace SecureFileSharingAPI;

public class BlockEvent
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ClientId { get; set; } = string.Empty;
    public string RuleTriggered { get; set; } = string.Empty;
    public DateTime BlockedUntil { get; set; }
}
