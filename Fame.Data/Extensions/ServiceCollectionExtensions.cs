using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Fame.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FameContext>(options => options.UseSqlServer(configuration.GetConnectionString("FameConnection")), ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(o =>
            {
                o.Configuration = configuration["FameConfig:Cache:Server"];
                o.InstanceName = configuration["FameConfig:Cache:InstanceName"];
            });
            return services;
        }
    }
}
