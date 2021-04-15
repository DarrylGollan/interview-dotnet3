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

        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var customer = await _customerService.GetCustomer(id);

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
                    if (customer.Id == null || customer.Id == Guid.Empty)
                    {
                        customer.Id = Guid.NewGuid();
                    }

                    var customerId = await _customerService.AddCustomer(customer);
                    if (customerId != Guid.Empty)
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

        [HttpPut("{customerId:guid}", Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, Customer updatedCustomer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!await _customerService.CustomerExists(customerId))
                    {
                        return NotFound();
                    }

                    updatedCustomer.Id = customerId;
                    await _customerService.UpdateCustomer(updatedCustomer);

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpDelete("{customerId:guid}", Name = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            try
            {
                if (customerId != Guid.Empty)
                {
                    if (!await _customerService.CustomerExists(customerId))
                    {
                        return NotFound();
                    }

                    await _customerService.DeleteCustomer(customerId);

                    return NoContent();
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
    }
}