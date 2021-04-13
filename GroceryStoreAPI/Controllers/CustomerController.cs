using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Controllers
{
    public class CustomerController : BaseAPIController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService ?? 
                throw new ArgumentNullException(nameof(customerService));
        }
        
        [HttpGet(Name = "GetCustomers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            var customers = _customerService.GetCustomers();

            if(customers == null)
            {
                return NotFound();
            }
            
            return Ok(customers);
        }

        [HttpGet("{id:int}", Name = "GetCustomerById")]
        public ActionResult GetCustomerById(int id)
        {
            var customer = _customerService.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}