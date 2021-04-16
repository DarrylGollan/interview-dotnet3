using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Models
{
    public class CustomerForUpdateDTO
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Name has a maximum of 20 characters")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100")]
        public int Age { get; set; }

        public string Address { get; set; }

        [EmailAddress(ErrorMessage = "Not a valid email address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }
    }
}
