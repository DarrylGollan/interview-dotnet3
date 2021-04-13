using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DbContexts
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {}

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(SeedCustomerTestData());
        }

        public List<Customer> SeedCustomerTestData()
        {
            var customers = new List<Customer>();
            using(StreamReader sr = new StreamReader(@"database.json"))
            {
                string data = sr.ReadToEnd();
                customers = JsonConvert.DeserializeObject<JSONDatabase>(data).customers;
            }

            return customers;
        }
    }
}
