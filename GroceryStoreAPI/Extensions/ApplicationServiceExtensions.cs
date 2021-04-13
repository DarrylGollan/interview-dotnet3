using GroceryStoreAPI.DbContexts;
using GroceryStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStoreAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddDbContext<CustomerContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("GroceryStoreConnection"));
            });

            return services;
        }
    }
}
