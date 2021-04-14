using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetCustomers();

                if (customers == null)
                {
                    return NotFound();
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}", Name = "GetCustomerById")]
        public async Task<ActionResult> GetCustomerById(int? id)
        {
            if(id == null || id < 1)
            {
                return BadRequest();
            }
            
            try
            {
                var customer = await _customerService.GetCustomer(id.Value);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost(Name = "CreateCustomer")]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customerId = await _customerService.AddCustomer(customer);
                    if (customerId > 0)
                    {
                        return CreatedAtRoute("CreateCustomer",
                            new { Id = customerId },
                            customer);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }
    }
}