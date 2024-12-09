using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
	public class İletisim
	{
		[Key]
		public int İletisimID { get; set; }
		public string Email { get; set; }
		public string Telefon { get; set; }
		public string Adres { get; set; }
	}
}
