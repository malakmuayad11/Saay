using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saay.Services.Interfaces;
using Saay.Services.Classes;
using Saay.Repository.Interfaces;
using Saay.Repository.Classes;
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

        public static IServiceCollection AddSaayServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskCategoryService, TaskCategoryService>();
            services.AddScoped<IPasswordHasher, ArgonPasswordHasher>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IGoalCategoryService, GoalCategoryService>();
            services.AddScoped<IGoalService, GoalService>();
            services.AddScoped<IHabitLogService, HabitLogService>();
            services.AddScoped<IHabitService, HabitService>();
            return services;
        }

        public static IServiceCollection AddSaayRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskCategoryRepository, TaskCategoryRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IGoalCategoryRepository, GoalCategoryRepository>();
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<IHabitLogRepository, HabitLogRepository>();
            services.AddScoped<IHabitRepository, HabitRepository>();
            return services;
        }

        public static IServiceCollection AddSaayCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("SaayCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://127.0.0.1:5500",
                            "http://localhost:5109"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
