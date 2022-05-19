using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RandomQuote.Models;

public class QuoteModel
{
    [Key]
    public int QuoteId { get; set; }
    public string? Author { get; set; }
    [Required]
    public User? User { get; set; }
    [Required]
    public string? Quote { get; set; }
    
}