using Duc_BTL.Data;
using Duc_BTL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Duc_BTL.Controllers
{
  public class HomeController : Controller
  {
    private readonly Duc_BTLContext _context;

    public HomeController(Duc_BTLContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      var products = _context.product.ToList();
      return View(products);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult News ()
    {
      return View();
    }
    public IActionResult Contact ()
    {
      return View();
    }
  }
}
