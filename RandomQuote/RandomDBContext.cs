using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RandomQuote.Models;

namespace RandomQuote;

public class RandomDbContext : IdentityDbContext<User>
{
    public RandomDbContext()
    {
    }
    public RandomDbContext(DbContextOptions<RandomDbContext> opts) : base(opts)
    {
    }
    public virtual DbSet<QuoteModel>? Quotes { get; set; }
}