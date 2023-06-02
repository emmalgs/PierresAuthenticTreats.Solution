using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PierresAuthenticTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierresAuthenticTreats.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly PierreContext _db;
    private readonly UserManager<AppUser> _userManager;

    public TreatsController(UserManager<AppUser> userManager, PierreContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      string userId = userId.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      AppUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Treat> userTreats = _db.Treats
                              .Where(entry => entry.User.Id == currentUser.Id)
                              .Include(treat => treat.JoinEntities)
                              .ThenInclude(join => join.Flavor)
                              .ToList();
      return ViewModels(userTreats);
    }
  }
}