using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
    public class About
    {
        [Key]
        public int AboutID { get; set; }
        public string Konu { get; set; }
    }
}
