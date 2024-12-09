using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
	public class Rezervasyon
	{
        [Key]
        public int RezervasyonID { get; set; }
        [Required]
        public string Adi{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Telefon { get; set; }
        [Required]
        public int Sayi { get; set; }
        public string Saat { get; set; }
        public DateTime Tarih { get; set; }





    }
}
