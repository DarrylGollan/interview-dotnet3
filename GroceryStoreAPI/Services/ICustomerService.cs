using GroceryStoreAPI.Entities;
using System.Collections.Generic;

namespace GroceryStoreAPI.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(int id);
    }
}
