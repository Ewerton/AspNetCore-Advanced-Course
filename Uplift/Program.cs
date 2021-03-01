using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;

namespace Uplift
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Se estiver rodando no Azure, vai ter a VaultUri
            var isRunningOnAzure = !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            if (isRunningOnAzure)
            {
                string vaultURI = Environment.GetEnvironmentVariable("VaultUri");
                if (!String.IsNullOrWhiteSpace(vaultURI))
                {
                    var keyVaultEndpoint = new Uri(vaultURI);

                    Host.CreateDefaultBuilder(args)
                        .ConfigureAppConfiguration((context, config) =>
                        {

                            config.AddAzureKeyVault(
                            keyVaultEndpoint,
                            new DefaultAzureCredential());
                        });
                }
            }

            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
