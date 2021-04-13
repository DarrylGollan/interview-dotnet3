using GroceryStoreAPI.DbContexts;
using GroceryStoreAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryStoreAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerContext _context;

        public CustomerService(CustomerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList<Customer>();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.FirstOrDefault(i => i.Id == id);
        }
    }
}
