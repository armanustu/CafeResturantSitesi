using CafeResturant.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CafeResturant.ViewComponents
{
	public class SideBarGaleri :ViewComponent
	{
		private readonly ApplicationDbContext _db;
        public SideBarGaleri(ApplicationDbContext db)
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
