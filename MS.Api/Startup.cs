

using Microsoft.OpenApi.Models;
using MS.Repository;
using MS.Repository.Interfaces;
using MS.Service;

namespace MS.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnectionString = Configuration["MongoDb:ConnectionString"];
            var mongoDatabaseName = Configuration["MongoDb:Database"];

            services.AddSingleton<IMongoDbConnection>(sp => new MongoDbConnection(mongoConnectionString, mongoDatabaseName));

            services.AddScoped<IMyEntityRepository, MyEntityRepository>();
            services.AddScoped<MyService>();
            services.AddControllers();

            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Microservice API", Version = "v1" });
            });
                services.AddSingleton<RabbitMqService>();
        }
             public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Microservice API V1");
                c.RoutePrefix = string.Empty;
            });
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}