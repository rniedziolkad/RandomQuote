using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class ChangePasswordViewModel
{
    [Required]
    [PasswordPropertyText]
    public string? Password { get; init; }
    [Required]
    [PasswordPropertyText]
    [MinLength(6, ErrorMessage = "Password needs to be at least 6 characters long")]
    [DisplayName("New password")]
    public string? NewPassword { get; init; }
    [PasswordPropertyText]
    [DisplayName("Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "Value doesn't match new password")]
    public string? ConfirmPassword { get; init; }
}