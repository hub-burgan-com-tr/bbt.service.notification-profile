using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.Infrastructure;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddHealthChecks();
       
    }
    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
       
        app.UseHealthChecks("/health");
        
    }
}