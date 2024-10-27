using Microsoft.AspNetCore.Mvc;

namespace Duc_BTL.Areas.Admin.Models.Components
{
    public class _SideBar : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
