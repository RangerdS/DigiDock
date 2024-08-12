using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Serilog;

namespace DigiDock.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args, configuration).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var backgroundJobs = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
                    backgroundJobs.Enqueue(() => Log.Information("Starting up the host"));
                }

                host.Run();
            }
            catch (Exception ex)
            {
                using (var scope = CreateHostBuilder(args, configuration).Build().Services.CreateScope())
                {
                    var backgroundJobs = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
                    backgroundJobs.Enqueue(() => Log.Fatal(ex, "Host terminated unexpectedly"));
                }
            }
            finally
            {
                using (var scope = CreateHostBuilder(args, configuration).Build().Services.CreateScope())
                {
                    var backgroundJobs = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
                    backgroundJobs.Enqueue(() => Log.CloseAndFlush());
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddConfiguration(configuration);
                });
    }
}