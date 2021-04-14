using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
