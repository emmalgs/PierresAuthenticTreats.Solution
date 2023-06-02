using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PierresAuthenticTreats.Models;
using System.Collections.Generic;
using System.Linq;

namespace PierresAuthenticTreats.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly PierreContext _db;

    public FlavorsController(PierreContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Flavor> allFlavors = _db.Flavors
                                  .Include(f => f.JoinEntities)
                                  .ThenInclude(join => join.Treat)
                                  .ToList();
      return View(allFlavors);
    }

    [HttpPost]
    public ActionResult Delete(int flavorId)
    {
      string referringUrl = Request.Headers["Referer"];
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == flavorId);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return Redirect(referringUrl);
    }

    [HttpPost]
    public ActionResult Edit(int flavorId, int treatId, string type)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == flavorId);
      thisFlavor.Type = type;
      _db.SaveChanges();
      return RedirectToAction("Edit", "Treats", new { id = treatId });
    }
  }
}
