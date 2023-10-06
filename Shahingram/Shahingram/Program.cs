using Autofac.Extensions.DependencyInjection;
using MyApi;
using NLog;
using NLog.Web;

namespace Shahingram
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Flush();
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureLogging(options => options.ClearProviders())
            .UseNLog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                //webBuilder.ConfigureLogging(options => options.ClearProviders());
                //webBuilder.UseNLog();
                webBuilder.UseStartup<Startup>();
            });
    }
}