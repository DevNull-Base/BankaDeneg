using System.ComponentModel.DataAnnotations;

namespace SharedModels;

public struct AuthRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}