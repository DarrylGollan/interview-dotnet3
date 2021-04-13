using FluentAssertions;
using GroceryStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        public async Task GetCustomerById_ExistingIdPassed_ReturnsCorrectItem()
        {
            // Arrange
            var url = "/";
            var testId = 1;

            // Act
            var response = await _client.GetAsync(_baseURL + url + testId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            Assert.Equal(testId, customer.Id);
        }
    }
}
