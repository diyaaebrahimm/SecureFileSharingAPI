using System;
using System.ComponentModel.DataAnnotations;

namespace SecureFileSharingAPI;

public class ClientStatus
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ClientId { get; set; } = string.Empty;
    public DateTime LastRequest { get; set; }
    public bool IsBlocked { get; set; }
}
