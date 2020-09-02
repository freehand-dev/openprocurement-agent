using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting;
using System.IO;
using openprocurement.api.client.Models;
using Microsoft.EntityFrameworkCore;

namespace openprocurement_agent
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSystemd()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.Sources.Clear();
                    IHostEnvironment env = builderContext.HostingEnvironment;

                    #region WorkingDirectory
                    var workingDirectory = env.ContentRootPath;
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        workingDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FreeHand", env.ApplicationName);
                    }
                    else if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        workingDirectory = System.IO.Path.Combine($"/opt/", env.ApplicationName, "etc", env.ApplicationName);
                    }
                    if (!System.IO.Directory.Exists(workingDirectory))
                        System.IO.Directory.CreateDirectory(workingDirectory);

                    config.SetBasePath(workingDirectory);

                    // add workingDirectory service configuration
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                       {"WorkingDirectory", workingDirectory}
                    });
                    #endregion

                    //
                    Console.WriteLine($"$Env:EnvironmentName={ env.EnvironmentName }");
                    Console.WriteLine($"$Env:ApplicationName={ env.ApplicationName }");
                    Console.WriteLine($"$Env:ContentRootPath={ env.ContentRootPath }");
                    Console.WriteLine($"WorkingDirectory={ workingDirectory }");

                    config.AddIniFile($"{ env.ApplicationName }.conf", optional: true, reloadOnChange: true);
                    config.AddCommandLine(args);
                    config.AddEnvironmentVariables();

                })
                .ConfigureServices((hostContext, services) =>
                {
                    //get WorkingDirectory from config
                    string workingDirectory = hostContext.Configuration.GetValue<string>("WorkingDirectory");

                    // Add DatabaseContex.
                    services.AddDbContext<Models.TenderHistoryDbContex>(options =>
                            options.UseSqlite($"Data Source=\"{System.IO.Path.Combine(workingDirectory, "TenderHistory.db")}\""));

                    // Add DatabaseContex.
                    services.AddDbContext<Models.ProcuringEntityDbContex>(options =>
                            options.UseSqlite($"Data Source=\"{System.IO.Path.Combine(workingDirectory, "ProcuringEntity.db")}\""));

                    // Configuration
                    services.Configure<Models.AppSettings>(hostContext.Configuration);

                    //OpenProcurement Service
                    services.AddHostedService<Services.OpenprocurementService>();

                })
                .ConfigureLogging((builderContext, logging) =>
                {
                    logging.AddConfiguration((IConfiguration)builderContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });
    }
}
