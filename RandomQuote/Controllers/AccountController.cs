using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomQuote.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RandomQuote.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /Account
    //[Authorize]
    public IActionResult Index()
    {
        Console.WriteLine(HttpContext.User);
        return View();
    }

    //GET: /Account/Login
    public IActionResult Login()
    {
        return View();
    }

    //GET: /Account/Register
    public IActionResult Register()
    {
        return View();
    }

    //GET: /Account/Visit/id
    public IActionResult Visit(string id)
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        Console.WriteLine("Logging In");
        Console.WriteLine(model.Username);
        Console.WriteLine(model.Password);

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        if (result == SignInResult.Success)
        {
            Console.WriteLine("Success Login");
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        else if (result == SignInResult.Failed)
        {
            Console.WriteLine("Filed login");
            ModelState.AddModelError("","Invalid login or password!");
        }
        
        return View();
    }
}