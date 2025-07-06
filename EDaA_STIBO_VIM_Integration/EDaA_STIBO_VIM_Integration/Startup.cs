using Azure.Core;
using Azure.Identity;
using Azure.Storage.Files.DataLake;
using EDaA_STIBO_VIM_Integration.Data.Adapter;
using EDaA_STIBO_VIM_Integration.Data.Interface;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

[assembly: FunctionsStartup(typeof(EDaA_STIBO_VIM_Integration.Startup))]
namespace EDaA_STIBO_VIM_Integration
{
    /// <summary>
    /// StartUp class
    /// </summary>
    [ExcludeFromCodeCoverage]
    class Startup : FunctionsStartup
    {
        #region private variable
        private readonly string accountName = Environment.GetEnvironmentVariable(Constants.Account_Name);
        #endregion
        /// <summary>
        /// Configure setting
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            builder.Services.AddSingleton<IConfiguration>(config);

            TokenCredential tokenCredential = new DefaultAzureCredential();

            string containerEndPoint = $"https://{accountName}.dfs.core.windows.net";
            var uri = new Uri(containerEndPoint);

            builder.Services.AddSingleton(s => new DataLakeServiceClient(uri, tokenCredential));
            builder.Services.AddTransient<IAdlsAdapter, AdlsAdapter>();
            builder.Services.AddTransient<ILoggerAdapter, LoggerAdapter>();
        }
    }
}
