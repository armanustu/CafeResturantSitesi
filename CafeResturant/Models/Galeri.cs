using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
	public class Galeri
	{
        [Key]
        public int GaleriID { get; set; }
        public string Image { get; set; }
    }
}
