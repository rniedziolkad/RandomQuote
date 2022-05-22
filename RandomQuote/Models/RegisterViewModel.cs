using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; init; }
    [Required]
    [RegularExpression("\\w*", ErrorMessage = "Can only contain letters, digits and _ sign")]
    public string? Username { get; init; }
    [Required]
    [PasswordPropertyText]
    [MinLength(6, ErrorMessage = "Password needs to be at least 6 characters long")]
    public string? Password { get; init; }
    [Compare("Password")]
    [DisplayName("Confirm password")]
    public string? ConfirmPassword { get; init; }
}