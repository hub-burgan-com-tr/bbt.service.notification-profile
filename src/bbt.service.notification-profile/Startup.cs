using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Microsoft.OpenApi.Models;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "bbt.service.notification-profile", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlTopic = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlTopic);

                c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
                c.CustomSchemaIds(x => x.FullName);
            });
        services.AddHealthChecks();
       
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "bbt.service.notification-profile v1");
                            c.RoutePrefix = "";

                        });
        }
        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
        
    }
}