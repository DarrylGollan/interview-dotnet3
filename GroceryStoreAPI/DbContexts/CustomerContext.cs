using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace GroceryStoreAPI.DbContexts
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(SeedCustomerTestData());
        }

        public List<Customer> SeedCustomerTestData()
        {
            var customers = new List<Customer>();
            using (StreamReader sr = new StreamReader(@"databaseNew.json"))
            {
                string data = sr.ReadToEnd();
                customers = JsonConvert.DeserializeObject<JSONDatabase>(data).customers;
            }

            return customers;
        }
    }
}
