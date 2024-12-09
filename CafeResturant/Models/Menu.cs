using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeResturant.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Ozel { get; set; }
        public double Price { get; set; }
        public int CategoryID { get; set; }
        
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
    }
}
