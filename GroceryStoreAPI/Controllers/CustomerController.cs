using AutoMapper;
using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Models;
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
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService ??
                throw new ArgumentNullException(nameof(customerService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerForDisplayDTO>>> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetCustomers();

                if (customers == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<CustomerForDisplayDTO>>(customers));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerForDisplayDTO>> GetCustomerById(Guid id)
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

                return Ok(_mapper.Map<CustomerForDisplayDTO>(customer));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost(Name = "CreateCustomer")]
        public async Task<ActionResult<CustomerForDisplayDTO>> CreateCustomer(CustomerForCreationDTO customerForCreationDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = _mapper.Map<Customer>(customerForCreationDTO);
                    
                    if (customer.Id == null || customer.Id == Guid.Empty)
                    {
                        customer.Id = Guid.NewGuid();
                    }

                    //var customer = 
                    var customerId = await _customerService.AddCustomer(customer);
                    if (customerId != Guid.Empty)
                    {
                        return CreatedAtRoute("CreateCustomer",
                            new { Id = customerId },
                            _mapper.Map<CustomerForDisplayDTO>(customer));
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
        public async Task<IActionResult> UpdateCustomer(Guid customerId, CustomerForUpdateDTO customerForUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!await _customerService.CustomerExists(customerId))
                    {
                        return NotFound();
                    }

                    var customer = _mapper.Map<Customer>(customerForUpdateDTO);

                    customer.Id = customerId;
                    await _customerService.UpdateCustomer(customer);

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