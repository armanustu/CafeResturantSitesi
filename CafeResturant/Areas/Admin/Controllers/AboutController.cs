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
    //Admin areadaki admin kontroller adminin yapacağı işleri gösterir. customerdaki kontroller ise veri tabanında gerçekleşen verileri çekip user tarfaına yani kullanıcı tarafına yansıtmaktır

    //Not:Viewcomponents klasörünü genel olarak oluştururuz Components dosyasını ise Oluşturulan Controllerın View klasörünün altında controlıra ait dosyanın altında Components klasörü oluştururuz ve Components klasörünün altın ViewComponentslerde oluşturulan klas adıyla klasörü oluştururuz buraya default viewi gömeriz ve foreach ile döndürürüz

    //Not:Toastı kullanmak için eğer kullancı tarafında gösterilmesini istersek Layout sayfası 	@await Component.InvokeAsync("NToastNotify") yapıştırırız ve kullanıcı kontrollırda konsuctorında gerekli atamalar yapıldıktan sonra context.Savechanges altına ypıştırırız

    //Not:Identity konusunda Identity klasörününün altında Account altında onunda altında Manage klasörüne View home altında ViewStart.cshtml yi alıp kl Manage klasörünün altına ekledik sonra _layout sayfasında else kısmındaki Layout değerine "/Views/Shared/_Layout.cshtml"; atadık.Sonra ChangePasword.cshtml.cs klasında display nameleri değiştirdik 
    //Identity frame workta admin kontrolleri yada kontroller kullanmaz kendisine kontrolleri kullanır yani ChangePassword.cshtml.cs içinde bulunan kodları kullanır bizi kod yazma zahmettinde kurtarır
    //Layout admin taraflı kullanıcılar için ALayout ise websitesi tarafı için kullanıldı

    //NOT login sayfasında giriş yaparken kullanıcı, admin ,sekreter linkleri rolleme ile gelecektir
    //Normal üye olan kullancıları ve admin taraflı olan kullancıları hepsini Asp.netUSer tablosunda tutmak identity framework yapısına uyar mı????
    //Kullanıcı mail arman@hotmail.com
    //şifre Arman1234*
    //admin şifresi sekreter@hotmail.com
    //Test2123*
    //Kullanıcı şifresi alper@hotmail.com
    //Alper123*

    [Area("Admin")]
    [Authorize]
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;
        public AboutController(ApplicationDbContext context,IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }

        // GET: Admin/About
        public async Task<IActionResult> Index()
        {
              return _context.Abouts != null ? 
                          View(await _context.Abouts.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Abouts'  is null.");
        }

        // GET: Admin/About/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.AboutID == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Admin/About/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/About/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AboutID,Konu")] About about)
        {
            if (ModelState.IsValid)
            {
                _context.Add(about);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Hakkımızda ekleme başarılı");
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Admin/About/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        // POST: Admin/About/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AboutID,Konu")] About about)
        {
            if (id != about.AboutID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(about);
                    await _context.SaveChangesAsync();
                    _toast.AddSuccessToastMessage("Güncelleme işlemi başarılı");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.AboutID))
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
            return View(about);
        }

        // GET: Admin/About/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.AboutID == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Admin/About/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Abouts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Abouts'  is null.");
            }
            var about = await _context.Abouts.FindAsync(id);
            if (about != null)
            {
                _context.Abouts.Remove(about);
            }
            
            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("silme işlemi başarılı");
            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(int id)
        {
          return (_context.Abouts?.Any(e => e.AboutID == id)).GetValueOrDefault();
        }
    }
}
