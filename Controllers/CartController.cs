using Microsoft.AspNetCore.Mvc;
using Duc_BTL.Areas.Admin.Models;
using Duc_BTL.Data;

namespace Duc_BTL.Controllers
{
  public interface ICartService
  {
    public cart getCartById(int cartId);
    public int getCartCountByUserId(int userId);
    public List<cart> getListCartByUserId(int userId);
  }
  public class CartController : Controller, ICartService
  {
    private readonly Duc_BTLContext _context;

    public CartController(Duc_BTLContext context)
    {
      _context = context;
    }
    public cart getCartById(int cartId)
    {
      cart cart = _context.cart.Where(x => x.Id == cartId).FirstOrDefault();
      return cart;
    }

    public int getCartCountByUserId(int userId)
    {
      if (userId == 0)
        return 0;
      else
      {
        int count = _context.cart.Where(x => x.UserId == userId && x.Status == false).Count();
        return count;
      }
    }
    public List<cart> getListCartByUserId(int userId)
    {
      List<cart> listCart = _context.cart.Where(x => x.UserId == userId && x.Status == false).ToList();
      return listCart;
    }

  }
}
