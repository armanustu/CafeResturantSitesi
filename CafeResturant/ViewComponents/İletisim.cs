using CafeResturant.Data;
using Microsoft.AspNetCore.Mvc;

namespace CafeResturant.ViewComponents
{
	public class İletisim:ViewComponent
	{
        private readonly ApplicationDbContext _db;
        public İletisim(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {

            var iletisim = _db.İletisims.ToList();
            return View(iletisim);
        } 
    }
}
