using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Duc_BTL.Areas.Admin.Models;
using Duc_BTL.Data;

namespace Duc_BTL.Controllers
{
  public interface ICategoryService
  {
    public int getProductCountByCateId (int cateId);
    public List<category> getListCategory();
    public List<SelectListItem> getSelectListCategory();
    public int getCateIdByCateName(string cateName);
  }
  public class CategoryController : Controller, ICategoryService
  {
    private readonly Duc_BTLContext _context;

    public CategoryController(Duc_BTLContext context)
    {
      _context = context;
    }
    public IActionResult Index()
    {
      return View();
    }
    public int getProductCountByCateId(int cateId)
    {
      int count = _context.product.Where(x => x.CategoryId == cateId).Count();
      return count;
    }
    public List<category> getListCategory()
    {
      var list = _context.category.ToList();
      return list;
    }

    public List<SelectListItem> getSelectListCategory ()
    {
      var list = _context.category.ToList();
      List<SelectListItem> rs = new List<SelectListItem>();
      foreach (var item in list)
      {
        rs.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
      }
      return rs;
    }
    public int getCateIdByCateName(string cateName)
    {
      var obj = _context.category.FirstOrDefault(x => x.Name == cateName);
      if (obj == null)
        return 0;
      return obj.Id;
    }
  }
}
