using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GroceryStoreAPI.Customer
{
    public class CustomerController
    {
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            // Not developed yet
            throw new NotImplementedException();
        }
    }
}