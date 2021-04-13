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
            var response = await _client.GetAsync(_baseURL);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers = JsonConvert.DeserializeObject<List<Customer>>(await response.Content.ReadAsStringAsync());
            customers.Should().HaveCount(3);
        }
    }
}
