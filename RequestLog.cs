using System;
using System.ComponentModel.DataAnnotations;

namespace SecureFileSharingAPI;

public class RequestLog
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ClientId { get; set; } = string.Empty;
    [Required]
    public string Endpoint { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
