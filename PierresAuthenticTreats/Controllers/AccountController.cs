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

    public ViewResult Index() => View();

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
        AppUser user = new AppUser { UserName = model.UserName };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
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
  }
}