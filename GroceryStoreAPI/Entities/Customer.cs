using System;
using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name has a maximum of 20 characters")]
        public string Name { get; set; }

        [Range(1,100,ErrorMessage = "Age must be between 1 and 100")]
        public int Age { get; set; }

        public string Address { get; set; }

        [EmailAddress(ErrorMessage = "Not a valid email address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
