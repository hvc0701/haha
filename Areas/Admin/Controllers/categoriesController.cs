using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Duc_BTL.Areas.Admin.Models;
using Duc_BTL.Data;

namespace Duc_BTL.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class categoriesController : Controller
  {
    private readonly Duc_BTLContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public categoriesController(Duc_BTLContext context, IWebHostEnvironment hostingEnvironment)
    {
      _context = context;
      _hostingEnvironment = hostingEnvironment;
    }

    // GET: Admin/categories
    public async Task<IActionResult> Index()
    {
      return View(await _context.category.ToListAsync());
    }

    // GET: Admin/categories/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var category = await _context.category
          .FirstOrDefaultAsync(m => m.Id == id);
      if (category == null)
      {
        return NotFound();
      }

      return View(category);
    }

    // GET: Admin/categories/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Admin/categories/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] category category)
    {
      if (ModelState.IsValid)
      {
        _context.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(category);
    }

    // GET: Admin/categories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var category = await _context.category.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }
      return View(category);
    }

    // POST: Admin/categories/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] category category)
    {
      if (id != category.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          
          _context.Update(category);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!categoryExists(category.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(category);
    }

    // GET: Admin/categories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var category = await _context.category
          .FirstOrDefaultAsync(m => m.Id == id);
      if (category == null)
      {
        return NotFound();
      }

      return View(category);
    }

    // POST: Admin/categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var category = await _context.category.FindAsync(id);
      if (category != null)
      {
        _context.category.Remove(category);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool categoryExists(int id)
    {
      return _context.category.Any(e => e.Id == id);
    }
  }
}
