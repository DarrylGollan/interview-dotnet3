using GroceryStoreAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(Guid id);
        Task<Guid> AddCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Guid customerId);
        Task<bool> CustomerExists(Guid customerId);
        Task<bool> Save();
    }
}
