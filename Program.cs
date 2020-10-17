using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using GoodJobGames.Data;
using GoodJobGames.Utilities.Constants;
using GoodJobGames.Utilities.Helper;
using NLog.Web;
using NLog;

namespace GoodJobGames
{
    public class Program
    {
        private static string appSettingsFile = "./appsettings.json";

        public static void Main(string[] args)
        {
            Logger logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            try
            {
                var config = new ConfigurationBuilder()
                                            .AddJsonFile(appSettingsFile, optional: false)
                                            .Build();

                logger.Info($"{AppConstants.SolutionName}.Api trigger migration");
                RunMigration(config);

                logger.Info($"{AppConstants.SolutionName}.Api is starting");
                var webHost = BuildWebHost(args, config);

                webHost.Run();
            }
            catch (Exception ex)
            {
                logger.Info($"{AppConstants.SolutionName}.Api is caught exception {ex}");
                logger.Fatal(ex);
            }
            finally
            {
                logger.Info($"{AppConstants.SolutionName}.Api is shutting down");
                LogManager.Shutdown();
            }
        }

        private static void RunMigration(IConfigurationRoot config)
        {
            var dataStorage = ConfigurationHelper.GetDataStorage(config);
            var dbMigrationEngine = new DbMigrationEngine();
            //dbMigrationEngine.MigrateDown(DataStorageTypes.SqlServer, dataStorage.ConnectionString, 3);
            dbMigrationEngine.MigrateUp(dataStorage);
        }

        private static IWebHost BuildWebHost(string[] args, IConfiguration c)
        {
            var urls = c.GetSection("AspNetCoreUrls").Get<string[]>();

            return WebHost.CreateDefaultBuilder(args)
                          .UseKestrel()
                          .UseContentRoot(Directory.GetCurrentDirectory())
                          .UseIISIntegration()
                          .UseStartup<Startup>()
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              config.SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile(appSettingsFile, false);
                          })
                          .UseUrls(urls)
                          .UseNLog()
                          .Build();
        }
    }
}
