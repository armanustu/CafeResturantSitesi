using CafeResturant.Data;
using CafeResturant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Diagnostics;

namespace CafeResturant.Areas.Customer.Controllers
{
	//Not:Viewcomponents klasörünü genel olarak oluştururuz Components dosyasını ise Oluşturulan Controllerın View klasörünün altında controlıra ait dosyanın altında Components klasörü oluştururuz ve Components klasörünün altın ViewComponentslerde oluşturulan klas adıyla klasörü oluştururuz buraya default viewi gömeriz ve foreach ile döndürürüz

	//Not:Toastı kullanmak için eğer kullancı tarafında gösterilmesini istersek Layout sayfası 	@await Component.InvokeAsync("NToastNotify") yapıştırırız ve kullanıcı kontrollırda konsuctorında gerekli atamalar yapıldıktan sonra context.Savechanges altına ypıştırırız
    //Not :CafeResturanta projesinin üzerine sağ tıkla identity new scafold item tıkla otomatikmen üyelol,çıkış mail doğrulama sayfaları gelir.Asp.NetUSer eklemek istediğimiz kişinin propertlerini Register.cshtml.cs içine propertleri yazarız ve değerlerle Asp.net user tablosuna kullanıcılar eklenir
    //Not Area demek Area alanın olduğumuzu gösterir

	[Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly IToastNotification _toast;
        private readonly IWebHostEnvironment _he;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext _db, IToastNotification toast, IWebHostEnvironment he)
        {
             db = _db;
            _logger = logger;
            _toast = toast;
            _he = he;
        }
        public IActionResult Test()
        {
            return View();
        }
        public IActionResult Index()
        {
            var menu = db.Menus.Where(i => i.Ozel).ToList();
            return View(menu);
        }

		public IActionResult CategoryDetails(int? id)
		{
			var menu = db.Menus.Where(i => i.CategoryID==id).ToList();
            ViewBag.KategoriID = id;
			return View(menu);
		}

		public IActionResult Menu()
        {
            var menu = db.Menus.ToList();
            return View(menu);
        }

		public IActionResult MenuDetails(int id)
		{
			var menu = db.Menus.Where(x=>x.MenuID==id).ToList();
			return View(menu);
		}
		public IActionResult Rezervasyon()
        {
            return View();
        }

        // POST: Admin/Rezervasyons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rezervasyon([Bind("RezervasyonID,Adi,Email,Telefon,Sayi,Saat,Tarih")] Rezervasyon rezervasyon)
        {
            if (ModelState.IsValid)
            {
               db.Add(rezervasyon);
                await db.SaveChangesAsync();
                
                _toast.AddSuccessToastMessage("Reservasyonunuz başarıyla oluşturuldu keyifli saatler dileriz...");
                return RedirectToAction(nameof(Index));
            }
            return View(rezervasyon);
        }
    
        public IActionResult SidebarGaleri()
        {
			var galeri = db.Galeris.ToList();
			return View(galeri);

		}
        public IActionResult Galery()
        {
            var galeri = db.Galeris.ToList();
            return View(galeri);
        }
        public IActionResult About()
        {
            var about = db.Abouts.ToList();
            return View(about);
        }
		public IActionResult Contact()
		{
			return View();
		}

		// POST: Admin/Contact/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Contact([Bind("ContactID,Name,Email,Telefon,Mesaj")] Contact contact)
		{
			contact.Tarih = DateTime.Now;
			if (ModelState.IsValid)
			{
				db.Add(contact);
				await db.SaveChangesAsync();
				_toast.AddSuccessToastMessage("Teşekkür ederiz mesajınız iletilmiştir...");
				return RedirectToAction(nameof(Index));
			}
			return View(contact);
		}



		//Blog create işlemi yapılıyor
		public IActionResult Blog()
        {
            return View();
        }

        //Blog create işlemi yapılıyor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Blog(Blog blog)
        {

            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(_he.WebRootPath, @"site\menu");
                var ext = Path.GetExtension(files[0].FileName);
                if (blog.Image != null)
                {
                    var imagePath = Path.Combine(_he.WebRootPath, blog.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                using (var filesStream = new FileStream(Path.Combine(uploads, filename + ext), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                blog.Image = @"\site\menu\" + filename + ext;
                db.Add(blog);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}