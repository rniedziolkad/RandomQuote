using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RandomQuote.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [RegularExpression("Male|Female", ErrorMessage = "Invalid sex")]
    public string? Sex { get; set; }
    
}