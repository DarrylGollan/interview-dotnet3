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
using System.Threading;
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
        public async Task Test01_Get_WhenCalled_ReturnsAllCustomers()
        {
            // Act
            var response = await _client.GetAsync(_url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await response.Content.ReadAsStreamAsync();
            IEnumerable<Customer> customers = stream.ReadAndDeserializeFromJson<IEnumerable<Customer>>();

            customers.Should().HaveCount(5);
        }

        #endregion

        #region Get Customer By Id Tests (/api/customer/{customerId})

        [Fact]
        public async Task Test02_GetCustomerById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Test03_GetCustomerById_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var customerId = Guid.Parse("EF0C4D03-12B4-4677-8EED-801CE18FC883");

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Test04_GetCustomerById_EmptyGuidPassed_ReturnsBadRequestResult()
        {
            // Arrange
            var customerId = new Guid();

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test05_GetCustomerById_ExistingIdPassed_ReturnsCorrectItem()
        {
            // Arrange
            var customerId = Guid.Parse("EF0C4D03-12B4-4677-8EED-801CE18FC883");

            // Act
            var response = await _client.GetAsync(_url + customerId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await response.Content.ReadAsStreamAsync();
            Customer customer = stream.ReadAndDeserializeFromJson<Customer>();

            Assert.Equal(customerId, customer.Id);
            Assert.Equal("Michelle", customer.Name);
            Assert.Equal(38, customer.Age);
            Assert.Equal("433 Dufferin Ave., London ON N6L 1Z7", customer.Address);
            Assert.Equal("michelle@gmail.com", customer.Email);
            Assert.Equal("226-268-6611", customer.Phone);
            Assert.Equal(DateTimeOffset.Parse("2021-04-15T09:00:00"), customer.CreatedDate.Value);
        }

        #endregion

        #region Add Customer Tests (/api/customer)

        [Fact]
        public async Task Test06_AddCustomer_InvalidObjectPassed_NamePropertyEmptyString_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "",
                Age = 44,
                Address = "Test Address",
                Email = "test@gmail.com",
                Phone = "519-345-1234",
                CreatedDate = null,
                UpdatedDate = null
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test07_AddCustomer_InvalidObjectPassed_NameExceedsMaximumLength_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Name property exceeds the maximum number of characters",
                Age = 44,
                Address = "Test Address",
                Email = "test@gmail.com",
                Phone = "519-345-1234",
                CreatedDate = null,
                UpdatedDate = null
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test08_AddCustomer_InvalidObjectPassed_AgePropertyOutOfRange_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Frank",
                Age = 144,
                Address = "Test Address",
                Email = "test@gmail.com",
                Phone = "519-345-1234",
                CreatedDate = null,
                UpdatedDate = null
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test09_AddCustomer_InvalidObjectPassed_EmailPropertyNotValidEmail_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Frank",
                Age = 41,
                Address = "Test Address",
                Email = "testemail",
                Phone = "519-345-1234",
                CreatedDate = null,
                UpdatedDate = null
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test10_AddCustomer_InvalidObjectPassed_PhonePropertyNotValidPhone_ReturnsBadRequest()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Frank",
                Age = 41,
                Address = "Test Address",
                Email = "test@gmail.com",
                Phone = "abcd1234",
                CreatedDate = null,
                UpdatedDate = null
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test11_AddCustomer_ValidObjectPassed_ReturnsCreatedResponseAndCorrectCustomer()
        {
            // Arrange
            var newCustomer = new Customer()
            {
                Id = Guid.Parse("9095778F-055B-4115-8D83-661864F620D9"),
                Name = "Bill",
                Age = 41,
                Address = "Test Address",
                Email = "test@gmail.com",
                Phone = "519-334-9876",
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow
            };
            var json = JsonConvert.SerializeObject(newCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var stream = await response.Content.ReadAsStreamAsync();
            Customer createdCustomer = stream.ReadAndDeserializeFromJson<Customer>();

            Assert.Equal(newCustomer.Id, createdCustomer.Id);
            Assert.Equal(newCustomer.Name, createdCustomer.Name);
            Assert.Equal(newCustomer.Age, createdCustomer.Age);
            Assert.Equal(newCustomer.Address, createdCustomer.Address);
            Assert.Equal(newCustomer.Email, createdCustomer.Email);
            Assert.Equal(newCustomer.Phone, createdCustomer.Phone);
            Assert.Equal(newCustomer.CreatedDate, createdCustomer.CreatedDate);
            Assert.Equal(newCustomer.UpdatedDate, createdCustomer.UpdatedDate);
        }

        #endregion

        #region Update Exising Customer Tests

        [Fact]
        public async Task Test12_UpdateCustomer_InvalidNameProperty_Return_BadRequest()
        {
            // Arrange
            var customerId = Guid.Parse("286ABD03-F3D8-49D4-AEF3-35CF3A9D66D4");

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();

            // Update customer name to empty string
            exisingCustomer.Name = string.Empty;

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test13_UpdateCustomer_InvalidNamePropertyExceedsMaximumLength_Return_BadRequest()
        {
            // Arrange
            var customerId = Guid.Parse("286ABD03-F3D8-49D4-AEF3-35CF3A9D66D4");

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
        public async Task Test14_UpdateCustomer_InvalidAgePropertyOutOfRange_Return_BadRequest()
        {
            // Arrange
            var customerId = Guid.Parse("286ABD03-F3D8-49D4-AEF3-35CF3A9D66D4");

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();

            // Update customer age out of range
            exisingCustomer.Age = 101;

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test15_UpdateCustomer_InvalidEmailProperty_Return_BadRequest()
        {
            // Arrange
            var customerId = Guid.Parse("286ABD03-F3D8-49D4-AEF3-35CF3A9D66D4");

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();

            // Update customer email to invalid email address
            exisingCustomer.Email = "testemail";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test16_UpdateCustomer_InvalidPhoneProperty_Return_BadRequest()
        {
            // Arrange
            var customerId = Guid.Parse("286ABD03-F3D8-49D4-AEF3-35CF3A9D66D4");

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();

            // Update customer name to exceed the maximum length of 20 characters
            exisingCustomer.Phone = "2345asd";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Test17_UpdateCustomer_ValidData_Returns_NoContentResult()
        {
            // Arrange
            var customerId = Guid.Parse("286ABD03-F3D8-49D4-AEF3-35CF3A9D66D4");

            // Act
            var getExistingCustomerResponse = await _client.GetAsync(_url + customerId);
            getExistingCustomerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var stream = await getExistingCustomerResponse.Content.ReadAsStreamAsync();
            Customer exisingCustomer = stream.ReadAndDeserializeFromJson<Customer>();

            // Update customer name
            exisingCustomer.Name = "Alexa";

            var json = JsonConvert.SerializeObject(exisingCustomer);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var updateCustomerResponse = await _client.PutAsync(_url + exisingCustomer.Id, payload);

            // Assert
            updateCustomerResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        #endregion

        #region Delete Existing Customer

        [Fact]
        public async Task Test18_Delete_Customer_ValidCustomerId_Return_OkResult()
        {
            //Arrange  
            var customerId = Guid.Parse("9095778F-055B-4115-8D83-661864F620D9");

            //Act  
            var response = await _client.DeleteAsync(_url + customerId);

            //Assert  
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Test19_Delete_Customer_InvalidCustomerId_Return_NotFoundResult()
        {
            //Arrange  
            var customerId = Guid.NewGuid();

            //Act  
            var response = await _client.DeleteAsync(_url + customerId);

            //Assert  
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }

    #endregion
}
