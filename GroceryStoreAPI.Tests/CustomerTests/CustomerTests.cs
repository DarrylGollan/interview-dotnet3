using FluentAssertions;
using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GroceryStoreAPI.CustomerTests
{
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    [Collection("Sequential")]
    public class CustomerTests : IClassFixture<WebApplicationFactory<GroceryStoreAPI.Startup>>
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public CustomerTests(WebApplicationFactory<GroceryStoreAPI.Startup> fixture)
        {
            _client = fixture.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001");
            _url = "/api/customer/";
        }

        #region Get All Customers Tests (/api/customer)

        [Fact]
        public async Task Get_WhenCalled_ReturnsAllCustomers()
        {
            // Act
            var response = await _client.GetAsync(_url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await response.Content.ReadAsStreamAsync();
            IEnumerable<Customer> customers = stream.ReadAndDeserializeFromJson<IEnumerable<Customer>>();

            customers.Should().HaveCount(3);
        }

        #endregion

        #region Get Customer By Id Tests (/api/customer/1)

        [Fact]
        public async Task GetCustomerById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var customerId = 99;

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCustomerById_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var customerId = 1;

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetCustomerById_0IdPassed_ReturnsBadRequestResult()
        {
            // Arrange
            var customerId = 0;

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetCustomerById_ExistingIdPassed_ReturnsCorrectItem()
        {
            // Arrange
            var customerId = 1;

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await response.Content.ReadAsStreamAsync();
            Customer customer = stream.ReadAndDeserializeFromJson<Customer>();
            
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
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddCustomer_InvalidObjectPassed_NameExceedsMaximumLength_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Name = "Name exceeds the maximum number of characters allowed"
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

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
            var response = await _client.PostAsync(_url, payload);

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
            var response = await _client.PostAsync(_url, payload);

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
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();
            
            // Update customer name
            exisingCustomer.Name = "Sandy";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateCustomer_InvalidData_Return_BadRequest()
        {
            // Arrange
            var customerId = 2;

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();
            
            // Update customer name to exceed the maximum length of 20 characters
            exisingCustomer.Name = "Name exceeds the maximum length of 20 characters";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Task_Update_InvalidData_Return_NotFound()
        {
            // Arrange
            var customerId = 2;

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();
            
            // Update customer
            exisingCustomer.Id = 99;
            exisingCustomer.Name = "Sandy";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Delete Existing Customer

        [Fact]
        public async Task Delete_Customer_ValidCustomerId_Return_OkResult()
        {
            //Arrange  
            var customerId = 3;

            //Act  
            var response = await _client.DeleteAsync(_url + customerId);

            //Assert  
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Customer_InvalidCustomerId_Return_NotFoundResult()
        {
            //Arrange  
            var customerId = 99;

            //Act  
            var response = await _client.DeleteAsync(_url + customerId);

            //Assert  
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }

        #endregion
}
