using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fame.Background
{
    public static class ServicesExtension
    {
        public static void AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("FameConnection")));
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
        }
    }
}
