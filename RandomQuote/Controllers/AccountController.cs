using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        var user = _userManager.GetUserAsync(User).Result;

        return View(user);
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

    //GET: /Account/Visit/username
    [Route("/Account/Visit/{username}")]
    public IActionResult Visit(string username)
    {
        var user = _userManager.FindByNameAsync(username).Result;
        if (user == null)
        {
            return new NotFoundResult();
        }
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
            ModelState.AddModelError("Failed", "Invalid username or password");
        }
        if(!string.IsNullOrEmpty(returnUrl))
            ViewBag.ReturnUrl = returnUrl;
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
        var user = new User {Email = model.Email, UserName = model.Username};
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            TempData["Message"] = "Successfully Registered";
            return RedirectToAction("Login");
        }
        foreach(var error in result.Errors.Reverse())
        {
            switch (error.Code)
            {
                case "DuplicateEmail":
                    ModelState.AddModelError("Email", error.Description);
                    break;
                case "DuplicateUserName":
                    ModelState.AddModelError("Username", error.Description);
                    break;
                case "InvalidUserName":
                    ModelState.AddModelError("Username", error.Description);
                    break;
                default:
                    if (error.Code.StartsWith("Password"))
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    else
                    {
                        Console.WriteLine(error.Description);
                    }
                    break;
            }
                
        }
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
    //POST: /Account/EditEmail
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult EditEmail(string email)
    {
        if (ModelState.IsValid)
        {
            return Json(email);
        }

        return Json(ModelState);
    }
    //POST: /Account/EditUserInfo
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult EditUserInfo([Bind("FirstName,LastName,Sex,Description")] User userInfo)
    {
        if (ModelState.IsValid)
        {
            return Json(userInfo);
        }

        Dictionary<string, ModelErrorCollection> response = new Dictionary<string, ModelErrorCollection>();
        foreach (var x in ModelState)
        {
            response.Add(x.Key, x.Value.Errors);
        }
        return Json(response);

    }
    

}