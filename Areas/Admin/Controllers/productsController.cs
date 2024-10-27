using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Duc_BTL.Areas.Admin.Models;
using Duc_BTL.Data;

namespace Duc_BTL.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class productsController : Controller
  {
    private readonly Duc_BTLContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;


    public productsController(Duc_BTLContext context, IWebHostEnvironment hostingEnvironment)
    {
      _context = context;
      _hostingEnvironment = hostingEnvironment;
    }

    // GET: Admin/products
    public async Task<IActionResult> Index()
    {
      var Duc_BTLContext = _context.product.Include(p => p.category);
      return View(await Duc_BTLContext.ToListAsync());
    }

    // GET: Admin/products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var product = await _context.product
          .Include(p => p.category)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (product == null)
      {
        return NotFound();
      }
      ViewBag.CategoryId = new SelectList(_context.category, "Id", "Name");
      return View(product);
    }

    // GET: Admin/products/Create
    public IActionResult Create()
    {
      ViewData["CategoryId"] = new SelectList(_context.category, "Id", "Name");
      return View();
    }

    // POST: Admin/products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,Image,Price,Quantity,CategoryId")] product product, IFormFile? fileInput)
    {
      if (ModelState.IsValid)
      {
        if (fileInput != null)
        {
					var extension = Path.GetExtension(fileInput.FileName);
					var fileName = Guid.NewGuid().ToString().Substring(0, 10 - extension.Length) + extension;
					var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "img", fileName);
					var fileOld = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "img", fileInput.FileName);
					// Kiểm tra xem tệp có tồn tại trong thư mục Image hay không
					if (System.IO.File.Exists(fileOld))
					{
						// Nếu tệp đã tồn tại, không cần tạo tên mới
						product.Image = fileInput.FileName;
					}
					else
					{
						// Nếu tệp không tồn tại, tạo một tên file mới và lưu tệp vào thư mục
						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await fileInput.CopyToAsync(stream);
						}
            // Lưu đường dẫn của tệp vào model
            product.Image = fileName;
					}
				}
        _context.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["CategoryId"] = new SelectList(_context.category, "Id", "Name", product.CategoryId);
      return View(product);
    }

    // GET: Admin/products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var product = await _context.product.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }
      ViewData["CategoryId"] = new SelectList(_context.category, "Id", "Name", product.CategoryId);
      return View(product);
    }

    // POST: Admin/products/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image,Price,Quantity,CategoryId")] product product, IFormFile? fileInput)
    {
      if (id != product.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          
					if (fileInput != null)
					{
            product.Image = "";
						var extension = Path.GetExtension(fileInput.FileName);
						var fileName = Guid.NewGuid().ToString().Substring(0, 10 - extension.Length) + extension;
						var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "img", fileName);
						var fileOld = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "img", fileInput.FileName);
						// Kiểm tra xem tệp có tồn tại trong thư mục Image hay không
						if (System.IO.File.Exists(fileOld))
						{
							// Nếu tệp đã tồn tại, không cần tạo tên mới
							product.Image = fileInput.FileName;
						}
						else
						{
							// Nếu tệp không tồn tại, tạo một tên file mới và lưu tệp vào thư mục
							using (var stream = new FileStream(filePath, FileMode.Create))
							{
								await fileInput.CopyToAsync(stream);
							}
							// Lưu đường dẫn của tệp vào model
							product.Image = fileName;
						}
					}
					_context.Update(product);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!productExists(product.Id))
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
      ViewData["CategoryId"] = new SelectList(_context.category, "Id", "Name", product.CategoryId);
      return View(product);
    }

    // GET: Admin/products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var product = await _context.product
          .Include(p => p.category)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (product == null)
      {
        return NotFound();
      }
      ViewData["CategoryId"] = new SelectList(_context.category, "Id", "Name");
      return View(product);
    }

    // POST: Admin/products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var product = await _context.product.FindAsync(id);
      if (product != null)
      {
        _context.product.Remove(product);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool productExists(int id)
    {
      return _context.product.Any(e => e.Id == id);
    }
  }
}
