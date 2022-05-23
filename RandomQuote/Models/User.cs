using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RandomQuote.Models;

public class User : IdentityUser
{
    [DisplayName("First Name")]
    public string? FirstName { get; set; }
    [DisplayName("Last Name")]
    public string? LastName { get; set; }
    [RegularExpression("Male|Female", ErrorMessage = "Invalid sex")]
    public string? Sex { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<QuoteModel> MyQuotes { get; } = new List<QuoteModel>();

}