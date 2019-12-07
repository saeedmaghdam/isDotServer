using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace isDotServer
{
    public class Program
    {
        public static Serilog.Core.Logger HubLogger;

        public static void Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .Build();

            //host.Run();

            HubLogger = new LoggerConfiguration()
                .WriteTo.File("logs\\hub.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            var log = new LoggerConfiguration()
                .WriteTo.File("logs\\main.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            log.Information("Program started successfully");

            //var host = CreateWebHostBuilder(args).Build();

            ////////////////////////////////////////////////////
            //var host = new WebHostBuilder()
            //.UseKestrel()
            //.UseContentRoot(Directory.GetCurrentDirectory())
            //.UseIISIntegration()
            //.UseStartup<Startup>()
            //.Build();

            //host.Run();

            ////////////////////////////////////////////////
            //// use this to allow command line parameters in the config
            //var configuration = new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .Build();


            //var hostUrl = configuration["hosturl"];
            //if (string.IsNullOrEmpty(hostUrl))
            //    hostUrl = "http://192.168.1.253:6000";


            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseUrls(hostUrl)   // <!-- this 
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .UseConfiguration(configuration)
            //    .Build();

            //host.Run();
            /////////////////////////////////////////////////////
            ///





            var host = CreateWebHostBuilder(args).Build();


            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseUrls("http://localhost:5000", "http://odin:5000", "http://192.168.1.253:5000")
            //    .UseIISIntegration()
            //    .UseStartup<Startup>()
            //    .Build();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

        private static void CreateDbIfNotExists(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<Models.Context>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
