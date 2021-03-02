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

        // I want to use appsetting when running local and AzureKeyVault when running on Azure, so i commented it
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((context, config) =>
        //        {
        //            var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
        //            config.AddAzureKeyVault(
        //            keyVaultEndpoint,
        //            new DefaultAzureCredential());
        //        })
        //            .ConfigureWebHostDefaults(webBuilder =>
        //            {
        //                webBuilder.UseStartup<Startup>();
        //            });




        //https://upliftvaultewer.vault.azure.net/

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Se estiver rodando no Azure, vai ter a WEBSITE_SITE_NAME
            var isRunningOnAzure = !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            IHostBuilder hostBuilderToConfigure = null;

            // Dependento do hosting onde a aplicação está rodando, obtenho os dados de configuração da app de forma diferente.
            // Se estiver rodando no Azure, obtenho do KeyVault, se estiver local, obtenho do AppSettings
            if (isRunningOnAzure)
                hostBuilderToConfigure = GetHostBuilderForAzure(args);
            else
                hostBuilderToConfigure = GetHostBuilderForLocalDeployment(args);


            return hostBuilderToConfigure.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }


        ///// <summary>
        ///// Creates an IHostBuilder reading the configuration from Azure Key Vault
        ///// </summary>
        ///// <param name="args"></param>
        ///// <returns></returns>
        private static IHostBuilder GetHostBuilderForAzure(string[] args)
        {
            IHostBuilder hostBuilderToReturn = null;
            string vaultURI = Environment.GetEnvironmentVariable("VaultUri");
            if (!String.IsNullOrWhiteSpace(vaultURI))
            {
                var keyVaultEndpoint = new Uri(vaultURI);

                hostBuilderToReturn = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((context, config) =>
                    {

                        config.AddAzureKeyVault(
                        keyVaultEndpoint,
                        new DefaultAzureCredential());
                    });
            }

            return hostBuilderToReturn;
        }


        ///// <summary>
        ///// Creates an IHostBuilder reading the configuration from appsetiiings
        ///// </summary>
        ///// <param name="args"></param>
        ///// <returns></returns>
        private static IHostBuilder GetHostBuilderForLocalDeployment(string[] args)
        {
            return Host.CreateDefaultBuilder(args);
        }

    }
}
