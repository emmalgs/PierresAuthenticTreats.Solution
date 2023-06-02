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
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      AppUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Treat> userTreats = _db.Treats
                              .Where(entry => entry.User.Id == currentUser.Id)
                              .Include(treat => treat.JoinEntities)
                              .ThenInclude(join => join.Flavor)
                              .ToList();
      return View(userTreats);
    }

    public ActionResult Create()
    {
      ViewBag.Flavors = _db.Flavors.Select(f => new SelectListItem
      {
        Value = f.FlavorId.ToString(),
        Text = f.Type
      }).ToList();
      return View();
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, string flavor)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      AppUser currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;

      #nullable enable
      Flavor? thisFlavor = _db.Flavors.FirstOrDefault(f => f.Type == flavor);
      #nullable disable
      if (thisFlavor == null)
      {
        thisFlavor = new Flavor() { Type = flavor};
        _db.Flavors.Add(thisFlavor);
        _db.SaveChanges();
      }
      
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(join => join.FlavorId == thisFlavor.FlavorId && join.TreatId == treat.TreatId);
      #nullable disable
      if (joinEntity == null && thisFlavor.FlavorId != 0)
      {
        _db.Treats.Add(treat);
        _db.SaveChanges();
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = thisFlavor.FlavorId, TreatId = treat.TreatId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats    
                          .Include(t => t.JoinEntities)
                          .ThenInclude(join => join.Flavor)
                          .FirstOrDefault(t => t.TreatId == id);
      return View(thisTreat);
    }
  }
}