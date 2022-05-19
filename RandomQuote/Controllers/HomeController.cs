using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomQuote.Models;

namespace RandomQuote.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RandomDbContext _context;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;

    public HomeController(ILogger<HomeController> logger, RandomDbContext context,
        UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize]
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