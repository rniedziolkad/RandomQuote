using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomQuote.Models;

namespace RandomQuote.Controllers;

public class HomeController : Controller
{
    private readonly RandomDbContext _context;
    private readonly UserManager<User> _userManager;

    public HomeController(RandomDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index(int? id)
    {
        QuoteModel? quote;
        if (id == null)
        {
            quote = _context.Quotes?.Include(model => model.User).Include(model=>model.UserLikes)
                .AsEnumerable().OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
        }
        else
        {
            quote = _context.Quotes?.Include(model => model.User).Include(model => model.UserLikes)
                .FirstOrDefault(model => model.QuoteId == id);
        }
        if (quote == null) return NotFound();
        var user = _userManager.GetUserAsync(User).Result;
        ViewBag.Liked = user != null && quote.UserLikes.Contains(user);
        return View(quote);
    }
    //POST: /Home/Like
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Like(int quoteId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Json(new {success = false, error = "Sign in to like quote"});
        }
        Console.WriteLine(quoteId);
        QuoteModel? quote = null;
        if (_context.Quotes != null)
        {
            quote = await _context.Quotes.FindAsync(quoteId);
        }
        Console.WriteLine(quote?.Quote);
        if (quote == null) return Json(new {success = false, error = "Could not add a like."});
        try
        {
            user.LikedQuotes.Add(quote);
            await _context.SaveChangesAsync();
            return Json(new {success = true});
        }
        catch (Exception e)
        {
            return Json(new {success = false, error = "Already liked!"});
        }
    }
    public async Task<IActionResult> DeleteLike(int quoteId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Json(new {success = false, error = "Sign in to continue"});
        }
        Console.WriteLine(quoteId);
        QuoteModel? quote = null;
        if (_context.Quotes != null)
        {
            quote = _context.Quotes.Include(model => model.UserLikes)
                .FirstOrDefault(q => q.QuoteId == quoteId);
        }
        if (quote == null) return Json(new {success = false, error = "Could not remove a like."});
        
        
        Console.WriteLine(quote.UserLikes.Count);
        quote.UserLikes.Remove(user);
        Console.WriteLine(quote.UserLikes.Count);
        
        await _context.SaveChangesAsync();
        return Json(new {success = true});

    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}