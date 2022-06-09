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
    
    //GET: /ManageQuotes
    public IActionResult Index()
    {
        var user = _userManager.GetUserAsync(User).Result;
        _randomDbContext.Quotes?.Include(quote => quote.UserLikes).Where(q=>q.User == user).Load();
        return View(user);
    }
    //GET: /ManageQuotes/AddQuote
    [HttpGet]
    [Authorize]
    public IActionResult Add()
    {
        return View();
    }
    //POST: /Account/Add
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind("Author,Quote")]QuoteModel model)
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

            model.User = user;
            _randomDbContext.Quotes.Add(model);
            await _randomDbContext.SaveChangesAsync();
            TempData["Success"] = "Successfully added quote";
            return RedirectToAction("Index");
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
    //GET: /ManageQuotes/Edit
    [HttpGet]
    [Authorize]
    public IActionResult Edit(int id)
    {
        var q = _randomDbContext.Quotes?.Include(quote => quote.UserLikes)
            .FirstOrDefault(model => model.QuoteId == id);
        if (q == null)
        {
            return NotFound();
        }
        var loggedUser = _userManager.GetUserAsync(User).Result;
        if (q.User != loggedUser)
        {
            return new ForbidResult();
        }
        return View(q);
    }
    //POST: /ManageQuotes/Edit
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([Bind("QuoteId,Author,Quote")]QuoteModel model)
    {
        if (!ModelState.IsValid) return View();
        
        _randomDbContext.Entry(model).State = EntityState.Modified;
        _randomDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    //DELETE: /ManageQuotes/Delete
    [HttpDelete]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int quoteId)
    {
        if (_randomDbContext.Quotes == null) return Json(new {succeeded = false, data = quoteId});
        
        var q = await _randomDbContext.Quotes.FindAsync(quoteId);
        if (q == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (q.User != user)
        {
            return new ForbidResult();
        }
        _randomDbContext.Entry(q).State = EntityState.Deleted;
        await _randomDbContext.SaveChangesAsync();
        TempData["Success"] = "Successfully deleted quote";
        return Json(new {succeeded = true, data = quoteId});

    }
}