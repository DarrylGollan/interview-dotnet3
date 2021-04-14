using FluentAssertions;
using GroceryStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GroceryStoreAPI.CustomerTests
{
    public class CustomerTests : IClassFixture<WebApplicationFactory<GroceryStoreAPI.Startup>>
    {
        private readonly HttpClient _client;
        private readonly string _baseURL;

        public CustomerTests(WebApplicationFactory<GroceryStoreAPI.Startup> fixture)
        {
            _client = fixture.CreateClient();
            _baseURL = "https://localhost:5001/api/customer";
        }

        #region Get All Customers Tests (/api/customer)

        [Fact]
        public async Task Get_WhenCalled_ReturnsAllCustomers()
        {
            // Act
            var response = await _client.GetAsync(_baseURL);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers = JsonConvert.DeserializeObject<List<Customer>>(await response.Content.ReadAsStringAsync());
            customers.Should().HaveCount(3);
        }

        #endregion

        #region Get Customer By Id Tests (/api/customer/1)

        [Fact]
        public async Task GetCustomerById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var url = "/99";

            // Act
            var response = await _client.GetAsync(_baseURL + url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCustomerById_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var url = "/1";

            // Act
            var response = await _client.GetAsync(_baseURL + url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetCustomerById_0IdPassed_ReturnsBadRequestResult()
        {
            // Arrange
            var url = "/";
            int customerId = 0;

            // Act
            var response = await _client.GetAsync(_baseURL + url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetCustomerById_ExistingIdPassed_ReturnsCorrectItem()
        {
            // Arrange
            var url = "/";
            int customerId = 1;

            // Act
            var response = await _client.GetAsync(_baseURL + url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            Assert.Equal(customerId, customer.Id);
            Assert.Equal("Bob", customer.Name);
        }

        #endregion

        #region Add Customer Tests (/api/customer)

        [Fact]
        public async Task AddCustomer_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Name = ""
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_baseURL, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddCustomer_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Name = "Darryl"
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_baseURL, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddCustomer_ValidObjectPassed_ReturnedResponseHasCreatedCustomer()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Name = "Michelle"
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_baseURL, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            Assert.Equal(newCustomer.Name, customer.Name);
        }

        #endregion

        #region Update Exising Customer Tests

        [Fact]
        public async Task UpdateCustomer_ValidData_Returns_NoContentResult()
        {
            // Arrange
            var customerId = 2;

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_baseURL + "/" + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var exisingCustomer = JsonConvert.DeserializeObject<Customer>(await getExistingCustomerResponse.Content.ReadAsStringAsync());
            
            // Update customer name
            exisingCustomer.Name = "Sandy";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_baseURL + "/" + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateCustomer_InvalidData_Return_BadRequest()
        {
            // Arrange
            var customerId = 2;

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_baseURL + "/" + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var exisingCustomer = JsonConvert.DeserializeObject<Customer>(await getExistingCustomerResponse.Content.ReadAsStringAsync());
            
            // Update customer name to exceed the maximum length of 20 characters
            exisingCustomer.Name = "Name exceeds the maximum length of 20 characters";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_baseURL + "/" + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Task_Update_InvalidData_Return_NotFound()
        {
            // Arrange
            var customerId = 2;

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_baseURL + "/" + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var exisingCustomer = JsonConvert.DeserializeObject<Customer>(await getExistingCustomerResponse.Content.ReadAsStringAsync());
            
            // Update customer
            exisingCustomer.Id = 99;
            exisingCustomer.Name = "Sandy";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_baseURL + "/" + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
