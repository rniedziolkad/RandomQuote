using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RandomQuote.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RandomQuote.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RandomDbContext _randomDbContext;
    
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RandomDbContext randomDbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _randomDbContext = randomDbContext;
    }

    // GET: /Account
    [HttpGet]
    [Authorize(Roles = "User")]
    public IActionResult Index()
    {
        if (User.Identity == null)
        {
            return View();
        }
        var user = _userManager.GetUserAsync(User).Result;
        _randomDbContext.Quotes?.Where(q=>q.User == user).Load();
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
    [HttpGet]
    [Route("/Account/Visit/{username}")]
    public IActionResult Visit(string username)
    {
        var user = _userManager.FindByNameAsync(username).Result;
        if (user == null)
        {
            return new NotFoundResult();
        }
        return View(user);
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
    public async Task<IActionResult> EditEmail(string email)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.SetEmailAsync(user, email);
            if (result.Succeeded)
            {
                TempData["EditSuccess"] = "Successfully updated email";
                return Json(new {succeeded = true, data=email});
            }
            //If there are errors add them to Model State
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code.EndsWith("Email") ? "Email" : error.Code, error.Description);
            }
        }
        //Extract errors from Model State in convenient format
        var errors = new Dictionary<string, ModelErrorCollection>();
        foreach (var (key, value) in ModelState)
        {
            errors[key] = value.Errors;
        }
        return Json(new{succeeded = false, errors, data=email});
    }
    //POST: /Account/EditUserInfo
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserInfo(EditUserInfoModel userInfo)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            user.FirstName = userInfo.FirstName;
            user.LastName = userInfo.LastName;
            user.Sex = userInfo.Sex;
            user.Description = userInfo.Description;
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["EditSuccess"] = "Successfully updated user information";
                return Json(new{succeeded = true, data=userInfo});
            }
            //If there are errors add them to Model State
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
        //Extract errors from Model State in convenient format
        var errors = new Dictionary<string, ModelErrorCollection>();
        foreach (var (key, value) in ModelState)
        {
            errors[key] = value.Errors;
        }
        return Json(new{succeeded = false, errors, data=userInfo});
    }
    //GET: /Account/ChangePassword
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _userManager.GetUserAsync(User);
        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
        if (result.Succeeded)
        {
            TempData["EditSuccess"] = "Successfully updated password";
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors.Reverse())
        {
            switch (error.Code)
            {
                case "PasswordMismatch":
                    ModelState.AddModelError("Password", error.Description);
                    break;
                default:
                    if (error.Code.StartsWith("Password"))
                    {
                        ModelState.AddModelError("NewPassword", error.Description);
                    }
                    break;
            }
            Console.WriteLine(error.Code);
            Console.WriteLine(error.Description);
        }

        return View();

    }
}