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
using NToastNotify;

namespace CafeResturant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RezervasyonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification toast;
        public RezervasyonsController(ApplicationDbContext context,IToastNotification _toast)
        {
            _context = context;
            toast = _toast;
        }

        // GET: Admin/Rezervasyons
        public async Task<IActionResult> Index()
        {
              return _context.Rezervasyons != null ? 
                          View(await _context.Rezervasyons.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Rezervasyons'  is null.");
        }

        // GET: Admin/Rezervasyons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rezervasyons == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons
                .FirstOrDefaultAsync(m => m.RezervasyonID == id);
            if (rezervasyon == null)
            {
                return NotFound();
            }

            return View(rezervasyon);
        }

        // GET: Admin/Rezervasyons/Create
        public IActionResult Create()
        {
            return View();
        }     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RezervasyonID,Adi,Email,Telefon,Sayi,Saat,Tarih")] Rezervasyon rezervasyon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervasyon);
                await _context.SaveChangesAsync();
                toast.AddSuccessToastMessage("Reservasyonunuz iletilmiştir keyifli saatler dileriz");
                return RedirectToAction(nameof(Index));
            }
            return View(rezervasyon);
        }

        // GET: Admin/Rezervasyons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rezervasyons == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons.FindAsync(id);
            if (rezervasyon == null)
            {
                return NotFound();
            }
            return View(rezervasyon);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RezervasyonID,Adi,Email,Telefon,Sayi,Saat,Tarih")] Rezervasyon rezervasyon)
        {
            if (id != rezervasyon.RezervasyonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervasyon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervasyonExists(rezervasyon.RezervasyonID))
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
            return View(rezervasyon);
        }

        // GET: Admin/Rezervasyons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rezervasyons == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons
                .FirstOrDefaultAsync(m => m.RezervasyonID == id);
            if (rezervasyon == null)
            {
                return NotFound();
            }

            return View(rezervasyon);
        }

        // POST: Admin/Rezervasyons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rezervasyons == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rezervasyons'  is null.");
            }
            var rezervasyon = await _context.Rezervasyons.FindAsync(id);
            if (rezervasyon != null)
            {
                _context.Rezervasyons.Remove(rezervasyon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervasyonExists(int id)
        {
          return (_context.Rezervasyons?.Any(e => e.RezervasyonID == id)).GetValueOrDefault();
        }
    }
}
