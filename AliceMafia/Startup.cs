using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AliceMafia
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}