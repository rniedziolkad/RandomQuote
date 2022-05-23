using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomQuote.Models;

namespace RandomQuote.Controllers;

public class ManageQuotesController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RandomDbContext _randomDbContext;
    
    public ManageQuotesController(UserManager<User> userManager, RandomDbContext randomDbContext)
    {
        _userManager = userManager;
        _randomDbContext = randomDbContext;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
    //GET: /Account/AddQuote
    [HttpGet]
    [Authorize]
    public IActionResult AddQuote()
    {
        return View();
    }
    //POST: /Account/AddQuote
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddQuote([Bind("Author,Quote")]QuoteModel model)
    {
        Console.WriteLine(model.Author);
        Console.WriteLine(model.Quote);
        Console.WriteLine(ModelState.ErrorCount);
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (_randomDbContext.Quotes == null)
            {
                TempData["Error"] = "Failed adding data to database";
                return View();
            }
            await _randomDbContext.Quotes.Where(q => q.User == user).LoadAsync();
            user.MyQuotes.Add(model);
            await _randomDbContext.SaveChangesAsync();
            Console.WriteLine("User quotes:");
            foreach (var quote in user.MyQuotes)
            {
                Console.WriteLine(quote.Author+":"+quote.Quote);
            }
            return View();
        }

        foreach (var error in ModelState)
        {
            Console.WriteLine(error.Key);
            foreach (var e in error.Value.Errors)
            {
                Console.WriteLine(e.ErrorMessage);
            }
            
        }
        return View();
    }
}