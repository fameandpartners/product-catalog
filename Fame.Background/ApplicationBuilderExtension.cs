using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Fame.Background
{
    public static class ApplicationBuilderExtension
    {
        public static void UseBackgroundService(this IApplicationBuilder app)
        {   
            app.UseHangfireServer( new BackgroundJobServerOptions() { WorkerCount = 1 });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() }
            });
        }
    }
}