using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace NoteUI.Services.Configure
{
    public class ConfigureService
    {
        public IConfiguration GetConfigFromAppsetting()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false);

            var configuration = builder.Build();

            return configuration;
        }
    }
}
