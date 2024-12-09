using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeResturant.Data;
using CafeResturant.Models;
using NToastNotify;
using Microsoft.AspNetCore.Authorization;

namespace CafeResturant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class GaleriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;
        private readonly IToastNotification _toast;
        public GaleriController(ApplicationDbContext context, IWebHostEnvironment he,IToastNotification toast)
        {
            _context = context;
            _he = he;
            _toast = toast;
        }

        // GET: Admin/Galeri
        public async Task<IActionResult> Index()
        {
              return _context.Galeris != null ? 
                          View(await _context.Galeris.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Galeris'  is null.");
        }

        // GET: Admin/Galeri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Galeris == null)
            {
                return NotFound();
            }

            var galeri = await _context.Galeris
                .FirstOrDefaultAsync(m => m.GaleriID == id);
            if (galeri == null)
            {
                return NotFound();
            }

            return View(galeri);
        }

        // GET: Admin/Galeri/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Galeri galeri)
        {
          
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(_he.WebRootPath, @"site\menu");
                    var ext = Path.GetExtension(files[0].FileName);
                    if (galeri.Image != null)
                    {
                        var imagePath = Path.Combine(_he.WebRootPath, galeri.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var filesStream = new FileStream(Path.Combine(uploads, filename + ext), FileMode.Create))
                    {
                        files[0].CopyTo(filesStream);
                    }
                    galeri.Image = @"\site\menu\" + filename + ext;
                }

                _context.Add(galeri);
                await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("Galeri ekleme başarılı");
                return RedirectToAction(nameof(Index));
          
            return View(galeri);
        }

        // GET: Admin/Galeri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Galeris == null)
            {
                return NotFound();
            }

            var galeri = await _context.Galeris.FindAsync(id);
            if (galeri == null)
            {
                return NotFound();
            }
            return View(galeri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Galeri galeri)
        {
            if (id != galeri.GaleriID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(_he.WebRootPath, @"site\menu");
                    var ext = Path.GetExtension(files[0].FileName);
                    if (galeri.Image != null)
                    {
                        var imagePath = Path.Combine(_he.WebRootPath, galeri.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var filesStream = new FileStream(Path.Combine(uploads, filename + ext), FileMode.Create))
                    {
                        files[0].CopyTo(filesStream);
                    }
                    galeri.Image = @"\site\menu\" + filename + ext;
                }
                _context.Update(galeri);
                 await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Güncelleme ekleme başarılı");
                return RedirectToAction(nameof(Index));
            }
            return View(galeri);
        }

        // GET: Admin/Galeri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Galeris == null)
            {
                return NotFound();
            }

            var galeri = await _context.Galeris
                .FirstOrDefaultAsync(m => m.GaleriID == id);
            if (galeri == null)
            {
                return NotFound();
            }

            return View(galeri);
        }

        // POST: Admin/Galeri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Galeris == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Galeris'  is null.");
            }
            var galeri = await _context.Galeris.FindAsync(id);
            if (galeri != null)
            {
                var imagePath = Path.Combine(_he.WebRootPath, galeri.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }


                _context.Galeris.Remove(galeri);
            }
            
            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("silme işlemi başarılı");
            return RedirectToAction(nameof(Index));
        }

        private bool GaleriExists(int id)
        {
          return (_context.Galeris?.Any(e => e.GaleriID == id)).GetValueOrDefault();
        }
    }
}
