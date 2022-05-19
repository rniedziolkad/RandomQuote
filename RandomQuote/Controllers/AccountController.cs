using Microsoft.AspNetCore.Mvc;

namespace RandomQuote.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index(string? id)
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
}