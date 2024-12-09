using CafeResturant.Data;
using Microsoft.AspNetCore.Mvc;

namespace CafeResturant.ViewComponents
{
	public class CategoryList :ViewComponent
	{
        public ApplicationDbContext _db;
        public CategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var category=_db.Categories.ToList();
            return View(category);
        }
    }
}
