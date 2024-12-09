using CafeResturant.Data;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Drawing.Drawing2D;

namespace CafeResturant.ViewComponents
{
	public class Galeri :ViewComponent
	{
        private readonly ApplicationDbContext _db;
        public Galeri(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var galeri = _db.Galeris.ToList();
            return View(galeri);
        }
    }
}
