using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeResturant.Data;
using CafeResturant.Models;
using Microsoft.AspNetCore.Authorization;

namespace CafeResturant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;

       

        public MenusController(ApplicationDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Admin/Menus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Menus.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MenuID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Admin/Menus/Create
        public IActionResult Create()
        {
            ////List<SelectListItem> valueMenu = (from m in _context.Menus.ToList()
            ////                                  select new SelectListItem
            ////                                  {
            ////                                      Text = m.Title,
            ////                                      Value = m.MenuID.ToString()
            ////                                  }).ToList();
            ////ViewBag.Menu = valueMenu;
            List<SelectListItem> valueCategory = (from m in _context.Categories.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = m.CategoryName,
                                                      Value = m.CategoryID.ToString()
                                                  }).ToList();
            ViewBag.Category = valueCategory;
            //ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuID,Title,Description,Image,Ozel,Price,CategoryID")] Menu menu)
        {

            
            List<SelectListItem> valueMenu = (from m in _context.Menus.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = m.Title,
                                                      Value = m.MenuID.ToString()
                                                  }).ToList();
            ViewBag.Menu = valueMenu;
            List<SelectListItem> valueCategory = (from m in _context.Categories.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = m.CategoryName,
                                                  Value = m.CategoryID.ToString()
                                              }).ToList();
            ViewBag.Category = valueCategory;

            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(_he.WebRootPath, @"site\menu");
                var ext = Path.GetExtension(files[0].FileName);
                if (menu.Image != null)
                {
                    var imagePath = Path.Combine(_he.WebRootPath, menu.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                using (var filesStream=new FileStream(Path.Combine(uploads,filename+ext),FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menu.Image=@"\site\menu\"+filename+ext;
            }

                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            //ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", menu.CategoryID);
            return View(menu);
        }

        // GET: Admin/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", menu.CategoryID);
            return View(menu);
        }

       
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Menu menu)
        {
           
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(_he.WebRootPath, @"site\menu");
                    var ext = Path.GetExtension(files[0].FileName);
                    if (menu.Image != null)
                    {
                        var imagePath = Path.Combine(_he.WebRootPath, menu.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var filesStream = new FileStream(Path.Combine(uploads, filename + ext), FileMode.Create))
                    {
                        files[0].CopyTo(filesStream);
                    }
                    menu.Image = @"\site\menu\" + filename + ext;
                }
            _context.Update(menu);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
            
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MenuID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Menus'  is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                var imagePath = Path.Combine(_he.WebRootPath, menu.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Menus.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return (_context.Menus?.Any(e => e.MenuID == id)).GetValueOrDefault();
        }
    }
}
