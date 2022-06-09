using System.ComponentModel.DataAnnotations;

namespace RandomQuote.Models;

public class QuoteModel
{
    public QuoteModel()
    {
        UserLikes = new List<User>();
    }
    [Key]
    public int QuoteId { get; init; }
    public string? Author { get; init; }
    public User? User { get; set; }
    [Required]
    public string? Quote { get; init; }

    public virtual ICollection<User> UserLikes { get; }

}