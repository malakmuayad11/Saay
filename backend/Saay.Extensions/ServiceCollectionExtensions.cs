using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Saay.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IConfiguration AddAzureKeyVaultIfConfigured(this IConfiguration config)
        {
            var keyVaultUrl = config["KeyVault:Url"];
            if (!string.IsNullOrWhiteSpace(keyVaultUrl))
            {
                if (config is ConfigurationManager mgr)
                {
                    mgr.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
                    return mgr;
                }

                var builder = new ConfigurationBuilder()
                    .AddConfiguration(config)
                    .AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
                return builder.Build();
            }
            return config;
        }

        public static IServiceCollection AddSaayPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connSecret = configuration["ConnectionString"];
            var connectionString = !string.IsNullOrWhiteSpace(connSecret)
                ? connSecret
                : configuration.GetConnectionString("SaayDB");

            services.AddDbContext<Saay.Data.SaayContext>(opt =>
                opt.UseSqlServer(connectionString));
            return services;
        }
    }
}
