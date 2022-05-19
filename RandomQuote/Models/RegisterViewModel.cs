using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    [PasswordPropertyText]
    public string? Password { get; set; }
    [Compare("Password")]
    [DisplayName("Confirm password")]
    public string? ConfirmPassword { get; set; }
}