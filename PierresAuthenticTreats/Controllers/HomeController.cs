using Microsoft.AspNetCore.Mvc;
using PierresAuthenticTreats.Models;

namespace PierresAuthenticTreats.Controllers;

public class HomeController : Controller
{
  [HttpGet("/")]
  public ActionResult Index()
  {
    return View();
  }
}