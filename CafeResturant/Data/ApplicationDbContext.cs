using CafeResturant.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CafeResturant.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Menu> Menus { get; set; }       
        public DbSet<Rezervasyon>Rezervasyons { get; set; }
        public DbSet<Galeri> Galeris { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<İletisim> İletisims { get; set; }
	}
}