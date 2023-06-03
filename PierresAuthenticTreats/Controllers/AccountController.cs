using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PierresAuthenticTreats.Models;
using PierresAuthenticTreats.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace PierresAuthenticTreats.Controllers
{
  public class AccountController : Controller
  {
    private readonly PierreContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, PierreContext db)
    {
      _db = db;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      AppUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Treat> userTreats = _db.Treats
                                  .Include(t => t.User)
                                  .Include(t => t.JoinEntities)
                                  .ThenInclude(join => join.Flavor)
                                  .Where(t => t.User == currentUser)
                                  .ToList();
      currentUser.OrderTotal = 0;
      foreach (Treat treat in userTreats)
      {
        currentUser.OrderTotal += treat.Price;
      }
      return View(userTreats);
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        AppUser user = new AppUser { UserName = model.UserName, Email = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          user.OrderTotal = 0;
          return RedirectToAction("Login");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
    }

    public ViewResult Login() => View();

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "Invalid user name or password");
          return View(model);
        }
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}