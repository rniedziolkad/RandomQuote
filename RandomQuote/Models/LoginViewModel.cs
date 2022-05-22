using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class LoginViewModel
{
    [Required]
    [DisplayName("Login")]
    public string? Username { get; init; }
    [Required]
    [PasswordPropertyText]
    public string? Password { get; init; }
    [Required]
    [DisplayName("Remember Me")]
    public bool RememberMe { get; init; }
}