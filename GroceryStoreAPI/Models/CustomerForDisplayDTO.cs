﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Models
{
    public class CustomerForDisplayDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
