using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Duc_BTL.Areas.Admin.Models;

namespace Duc_BTL.Data
{
  public class Duc_BTLContext : DbContext
  {
    public Duc_BTLContext(DbContextOptions<Duc_BTLContext> options)
        : base(options)
    {
    }

    public DbSet<Duc_BTL.Areas.Admin.Models.user> user { get; set; }
    public DbSet<Duc_BTL.Areas.Admin.Models.category> category { get; set; }
    public DbSet<Duc_BTL.Areas.Admin.Models.product> product { get; set; }
    public DbSet<Duc_BTL.Areas.Admin.Models.cart> cart { get; set; }
  }
}
