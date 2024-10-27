using Microsoft.AspNetCore.Mvc;

namespace Duc_BTL.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
