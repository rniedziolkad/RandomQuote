using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class LoginViewModel
{
    [Required]
    [DisplayName("Login")]
    public string? Username { get; set; }
    [Required]
    [PasswordPropertyText]
    public string? Password { get; set; }
    [Required]
    [DisplayName("Remember Me")]
    public bool RememberMe { get; set; }
}