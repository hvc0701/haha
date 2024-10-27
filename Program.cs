using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Duc_BTL.Data;
using Duc_BTL.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace Duc_BTL
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.AddDbContext<Duc_BTLContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("Duc_BTLContext") ?? throw new InvalidOperationException("Connection string 'Duc_BTLContext' not found.")));

      // Add services to the container.
      builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

      builder.Services.AddScoped<ICategoryService, CategoryController>();
      builder.Services.AddScoped<IProductService, ProductsController>();
      builder.Services.AddScoped<ICartService, CartController>();
      builder.Services.AddScoped<IUserService, UserController>();

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(options =>
		{
			options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
			options.SlidingExpiration = true;
			options.AccessDeniedPath = "/Forbidden/";
		});

			var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      });
      

      app.Run();
    }
  }
}
