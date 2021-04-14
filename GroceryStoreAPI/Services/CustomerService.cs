using GroceryStoreAPI.DbContexts;
using GroceryStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerContext _context;

        public CustomerService(CustomerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync<Customer>();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            //customer.CreatedDate = DateTimeOffset.Now;

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return customer.Id;
        }

        public async Task UpdateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(int customerId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);

            if(customer != null)
            {
                _context.Entry(customer).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CustomerExists(int customerId)
        {
            if(customerId < 1)
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return await _context.Customers.AnyAsync(c => c.Id == customerId);
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
