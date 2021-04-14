using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name has a maximum of 20 characters")]
        public string Name { get; set; }
    }
}
