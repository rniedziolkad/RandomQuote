using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class EditUserInfoModel
{

    [DisplayName("First Name")]
    public string? FirstName { get; init; }
    [DisplayName("Last Name")]
    public string? LastName { get; init; }
    [RegularExpression("Male|Female", ErrorMessage = "Invalid sex")]
    public string? Sex { get; init; }
    public string? Description { get; init; }
}