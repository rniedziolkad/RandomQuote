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
    [Authorize(Roles = "User")]
    public IActionResult Index()
    {
        if (User.Identity == null)
        {
            return View();
        }
        Console.WriteLine(User.Identity.Name);
        Console.WriteLine(User.Identity.IsAuthenticated);
        Console.WriteLine(User.IsInRole("User"));

        return View();


    }

    //GET: /Account/Login
    public IActionResult Login(string? returnUrl)
    {
        if(!string.IsNullOrEmpty(returnUrl))
            ViewBag.ReturnUrl = returnUrl;
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
    //POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, [Bind("returnUrl")] string? returnUrl)
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
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Console.WriteLine("Redirecting back");
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        else if (result == SignInResult.Failed)
        {
            Console.WriteLine("Failed login");
            ModelState.AddModelError("","Invalid login or password!");
        }
        
        return View();
    }
    //POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        Console.WriteLine("Register");
        Console.WriteLine(model.Email);
        Console.WriteLine(model.Username);
        Console.WriteLine(model.Password);
        User user = new User {Email = model.Email, UserName = model.Username};
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            Console.WriteLine("Success Register");
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Login");
        }
        Console.WriteLine(result.Errors);
        return View();
    }
    //POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}