using GroceryStoreAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(int id);
        Task<int> AddCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task<bool> CustomerExists(int customerId);
        Task<bool> Save();
    }
}
