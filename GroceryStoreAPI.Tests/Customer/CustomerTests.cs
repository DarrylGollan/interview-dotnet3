using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace GroceryStoreAPI.Customer
{
    public class CustomerTests
    {
        private CustomerController _customerController;

        public CustomerTests()
        {
            _customerController = new CustomerController();
        }
        
        [Fact]
        public void Get_WhenCalled_ReturnsAllCustomers()
        {
            // Act
            var result = _customerController.GetCustomers().Result as OkObjectResult;

            // Assert
            var customers = Assert.IsType<List<Customer>>(result.Value);
            Assert.Equal(3, customers.Count);
        }
    }
}
