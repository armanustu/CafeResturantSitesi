using System.ComponentModel.DataAnnotations;

namespace CafeResturant.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
