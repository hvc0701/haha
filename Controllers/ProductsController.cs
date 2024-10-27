using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Duc_BTL.Areas.Admin.Models;
using Duc_BTL.Data;
using System.Collections.Generic;

namespace Duc_BTL.Controllers
{
  public interface IProductService
  {
    public product getProductById(int productId);
  }
  public class ProductsController : Controller, IProductService
  {
    private readonly Duc_BTLContext _context;
    public ProductsController(Duc_BTLContext context)
    {
      _context = context;
    }

    public IActionResult Shop(string? cateId, string? keywords, string? sortBy, string? priceValue)
    {
      var products = _context.product.Include(x => x.category).ToList();
      if (cateId != null)
      {
        products = _context.product.Include(x => x.category).Where(x => x.CategoryId == int.Parse(cateId)).ToList();
      }
      if (keywords != null)
      {
        products = products.Where(x => x.Name.ToLower().Contains(keywords.ToLower().Trim())).ToList();
      }
      if (priceValue != null)
      {
        int minVal = int.Parse(priceValue.Split("_")[0]);
        int maxVal = int.Parse(priceValue.Split("_")[1]);
        products = products.Where(x => x.Price >= minVal && x.Price <= maxVal).ToList();
      }

      switch (sortBy)
      {
        case "default":
          products = products.OrderBy(x => x.Id).ToList();
          break;
        case "price_increase":
          products = products.OrderBy(x => x.Price).ToList();
          break;
        case "price_decrease":
          products = products.OrderByDescending(x => x.Price).ToList();
          break;
      }

      ViewBag.Categories = _context.category.ToList();
      ViewBag.sortBy = new List<SelectListItem>()
      {
        new SelectListItem { Value = "default", Text = "Mặc định"},
        new SelectListItem { Value = "price_increase", Text = "Giá tăng dần"},
        new SelectListItem { Value = "price_decrease", Text = "Giá giảm dần"}
      };
      return View(products);
    }

    public IActionResult Details(int id)
    {
      var product = _context.product.Include(x => x.category).FirstOrDefault(x => x.Id == id);
      ViewBag.RelatedProduct = _context.product.Include(x => x.category).Where(x => x.Id != id && x.CategoryId == product.CategoryId).ToList();
      return View(product);
    }

    public IActionResult Filter(string? keywords, string? sortBy)
    {
      string url = "/products/shop";
      Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
      keyValuePairs["keywords"] = keywords ?? string.Empty;
      keyValuePairs["sortBy"] = sortBy ?? string.Empty;

      foreach (var item in keyValuePairs)
      {
        // Kiểm tra xem tham số có khác null hoặc rỗng không
        if (!string.IsNullOrEmpty(item.Value) && item.Value != null && item.Value != "")
        {
          if (url == "/products/shop")
            url += $"?{item.Key}={item.Value}";
          else
            url += $"&{item.Key}={item.Value}";
        }
      }
      return Json(new { url = url });
    }

    public product getProductById(int productId)
    {
      var product = _context.product.Include(x => x.category).FirstOrDefault(x => x.Id == productId);
      return product;
    }
  }
}
