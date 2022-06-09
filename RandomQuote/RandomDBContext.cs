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
    public DbSet<QuoteModel>? Quotes{ get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasMany(user => user.LikedQuotes)
            .WithMany(quote => quote.UserLikes);

        builder.Entity<User>()
            .HasMany(user => user.MyQuotes)
            .WithOne(quote => quote.User);
        
        base.OnModelCreating(builder);
    }
}