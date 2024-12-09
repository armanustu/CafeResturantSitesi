using CafeResturant.Data;
using CafeResturant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeResturant.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        //https://localhost:7183/Admin/User/Index
        //Bu kod bizi login sayfasına yönlendirir
        //Kullanıcı buraya giriş yaparken admin olduğunu nasıl anlıyor login sayfasında nasıl geliyor??
        //Not:sekter olarak giriş yaparsak ındek sayfası açılır yani indekte kullanıcılar gelir ındekse bağlı layoutta ise admin olarak giriş yapamadığımız için Layoutta kullanıcı linki görünmez çünkü User.IsInRol değeri admin değil
        //Not:demekki  @if (User.IsInRole(diger.Role_admin)) ve [Authorize(Roles = diger.Role_sekreter)] loginle giriş yaparken bu kodlar tarafından tanınıyor ve kontrolü sağlanıyor
        // [Authorize(Roles = diger.Role_Admin)] ve @if (User.IsInRole(diger.Role_admin)) admin olarak giriş yaptığımız hem indekste hem layout hem link, hemde indekste sayfasında kullanıcılar gelir
        //Admin olarak loginde armagan@hotmail.com şifre Armagan123* giriş yaptığımızda ve Roles=diger.Role_admin atanmışsa ve layoutta user.IsInrole(diger.role_admin) linki verilmişse iki kullancılar hemde User.IsInRole() ait link gelir 

        //Normal üye olan kullancıları ve admin taraflı olan kullancıları hepsini Asp.netUSer tablosunda tutmak identity framework yapısına uyar mı????
        //Kullanıcı mail arman@hotmail.com
        //şifre Arman12345*
        //admin şifresi sekreter@hotmail.com
        //Test2123*
		//Kullanıcı şifresi alper@hotmail.com
		//Alper123*
        [Authorize(Roles = diger.Role_sekreter)]
        public IActionResult Index()
        {

            var users = _context.ApplicationUsers.ToList();
            var role = _context.Roles.ToList();
            var userRol = _context.UserRoles.ToList();
			foreach (var item in users)
			{
				var roleID = userRol.FirstOrDefault(i => i.UserId == item.Id).RoleId;
				item.Role = role.FirstOrDefault(u => u.Id == roleID).Name;
			}
			return View(users);           
        }
		
		public async Task<IActionResult> Delete(string? id)
		{
			if (id == null || _context.ApplicationUsers == null)
			{
				return NotFound();
			}

			var user = await _context.ApplicationUsers
				.FirstOrDefaultAsync(m => m.Id == id.ToString());
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

	
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (_context.ApplicationUsers == null)
			{
				return Problem("Entity set 'ApplicationDbContext.ApplicationUsers'  is null.");
			}
			var user = await _context.ApplicationUsers.FindAsync(id);
			if (user != null)
			{
				_context.ApplicationUsers.Remove(user);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}











	}
}
