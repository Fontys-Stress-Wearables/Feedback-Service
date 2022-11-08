using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Feedback_Service;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // //registers the mongo client and Imongo database
        // services.AddMongo()
        //         .AddRepository<Medicine>("medicines") 
        //         .AddMassTransitWithRabbitMQ();

        //the options SuppressAsyncSuffixInActionNames is so that asp.net does not remove the async name of methods by runtime
        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Feedback_Service", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Care.MedicineInventory.Service v1"));

            // //added for CORS
            // app.UseCors(builder =>
            // {
            //     builder.WithOrigins(Configuration[AllowedOriginSetting])
            //             .AllowAnyHeader()
            //             .AllowAnyMethod();
            // });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();
        app.UseDeveloperExceptionPage();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
