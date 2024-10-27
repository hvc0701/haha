
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Duc_BTL.Areas.Admin.Models;
using Duc_BTL.Data;
using Duc_BTL.Models;
using System.Drawing;
using System.Security.Claims;

namespace Duc_BTL.Controllers
{
  public class CartIdListRequest
  {
    public string CartIdList { get; set; }
  }
  public class ShoppingController : Controller
  {
    private readonly Duc_BTLContext _context;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    public ShoppingController(Duc_BTLContext context, IProductService productService, IUserService userService, ICartService cartService)
    {
      _context = context;
      _productService = productService;
      _cartService = cartService;
    }
    public IActionResult Index()
    {
      return View();
    }
    
    public async Task<IActionResult> ShoppingCart()
    {
      return View();
    }


    public IActionResult AddToCart(int productIdOrder, int quantity)
    {
      string url = Request.Headers["Referer"].ToString();
      if (User.Identity.IsAuthenticated)
      {
        var product = _productService.getProductById(productIdOrder);
        int userId = int.Parse(User.FindFirstValue("Id"));
        cart cartOld = _context.cart.Where(x => x.UserId == userId && x.ProductId == product.Id && x.Status == false).FirstOrDefault();
        if (cartOld != null)
        {
          cartOld.Quantity += quantity;
          cartOld.Total += (float)(product.Price * quantity);
          _context.Update(cartOld);
          _context.SaveChanges();
        }
        else
        {
          cart cart = new cart()
          {
            ProductId = product.Id,
            UserId = userId,
            Quantity = quantity,
            Total = (float)(product.Price * quantity),
            Status = false
          };
          _context.Add(cart);
          _context.SaveChanges();
        }
      }
      else
      {
        url = "/home/error";
      }
      
      // Chuyển hướng đến trang trước đó
      return Json(new { url = url });
    }

    [HttpPost]
    public IActionResult UpdateCart(int cartId, int quantity)
    {
      cart cartOld = _cartService.getCartById(cartId);
      var product = _productService.getProductById(cartOld.ProductId);
      if (cartOld != null)
      {
        
        cartOld.Quantity += quantity;
        cartOld.Total += (float)(product.Price * quantity);

        _context.Update(cartOld);
        _context.SaveChanges();
      }
      string url = Request.Headers["Referer"].ToString();
      // Chuyển hướng đến trang trước đó
      return Json(new {url = url });
    }

    [HttpGet]
    public IActionResult RemoveFromCart(int id)
    {
      cart cart = _cartService.getCartById(id);
      _context.Remove(cart);
      _context.SaveChanges();

      string referer = Request.Headers["Referer"].ToString();
      // Chuyển hướng đến trang trước đó
      return Redirect(referer);
    }
  }
}
