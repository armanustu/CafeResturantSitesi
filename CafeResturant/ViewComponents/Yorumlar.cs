using CafeResturant.Data;
using Microsoft.AspNetCore.Mvc;

namespace CafeResturant.ViewComponents
{
    public class Yorumlar :ViewComponent
    {
        private readonly ApplicationDbContext db;
        public Yorumlar(ApplicationDbContext _db)
        {
            db = _db;
                
        }
        public IViewComponentResult Invoke()
        {
            var yorumlar = db.Blogs.Where(i=>i.Onay).ToList();
            return View(yorumlar);
        }
    }
}
