using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Duc_BTL.Areas.Admin.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Duc_BTL.Data;
using Humanizer.Localisation.NumberToWords;
using Microsoft.AspNetCore.Http.Extensions;

namespace Duc_BTL.Controllers
{
  public interface IUserService
  {
    public user getUserByUserId(int userId);
  }
  public class UserController : Controller, IUserService
  {
    private readonly Duc_BTLContext _context;

    public UserController(Duc_BTLContext context)
    {
      _context = context;
    }
    public IActionResult MyAccount ()
    {
      if (User.Identity.IsAuthenticated)
      {
        int userId = int.Parse(User.FindFirstValue("Id"));
        user us = _context.user.FirstOrDefault(x => x.Id == userId);
        return View(us);
      }
      return Redirect("/user/login");
    }
    public IActionResult Edit (int? id)
    {
      var user = _context.user.FirstOrDefault(x => x.Id == id);
      return View(user);
    }

    [HttpPost]
    public IActionResult Edit(int id, string name, string email, string phone, string address)
    {
      var user = _context.user.FirstOrDefault(x => x.Id == id);
      user.Name = name;
      user.Email = email;
      _context.Update(user);
      _context.SaveChanges();
      return Redirect("/user/myaccount");
    }

    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
      var user = _context.user.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
      if (user != null)
      {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        return Redirect("/");

      }
      string message = "Tài khoản, mật khẩu không hợp lệ!";
      ViewBag.Message = message;
      return View();
    }

    public IActionResult LogOut()
    {
      HttpContext.SignOutAsync(
      CookieAuthenticationDefaults.AuthenticationScheme);
      return Redirect("/");
    }
    public IActionResult Register ()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Register(string name, string email, string password, string repassword)
    {
      var user = _context.user.Where(u => u.Email == email).FirstOrDefault();
      bool checkEmail = user == null; // true neu chua ton tai email
      if (ModelState.IsValid && password == repassword && checkEmail)
      {
        user us = new user
        {
          Name = name,
          Email = email,
          Password = password,
          Role = "KH"
        };
        _context.Add(us);
        _context.SaveChanges();
        ViewBag.Message = "Đăng ký thành công!";
        ViewBag.Status = true;
        return Redirect("/user/login"); 
      }
      ViewBag.Status = false;
      if (checkEmail == false)
      {
        ViewBag.Message = "Email này đã đăng ký tài khoản vui lòng thử lại!";
      }
      else
      {
        ViewBag.Message = "Vui lòng nhập mật khẩu trùng nhau!";
      }
      if (ViewBag.Status == false)
      {
        return View();
      }
      else
      {
        return Redirect("/user/login");
      }
    }

    public user getUserByUserId(int userId)
    {
      user us = _context.user.FirstOrDefault(x => x.Id == userId);
      return us;
    }
  }
}
