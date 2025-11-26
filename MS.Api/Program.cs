using MS.Api;
using MS.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var host = CreateHostBuilder(args).Build();

        Task.Run(() =>
        {
           var scope = host.Services.CreateScope();
              var rabbitMqService = scope.ServiceProvider.GetRequiredService<RabbitMqService>(); 
        });

        host.Run();
    }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   webBuilder.UseStartup<Startup>();
                });
        }
