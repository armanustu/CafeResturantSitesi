using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
     
        public string Image { get; set; }
        [Required]
        public bool Onay { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Tarih  { get; set; }
    }
}
