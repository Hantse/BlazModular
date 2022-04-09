using BlazModular.Business;
using BlazModular.Entities;
using BlazModular.Interfaces;
using BlazModular.Repositories;
using DbUp;
using DbUp.Engine;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace BlazModular
{
    public static class Bootstrap
    {
        public static WebApplicationBuilder RegisterBlazModularServer(this WebApplicationBuilder builder)
        {
            var assembly = Assembly.Load("BlazModular");
            builder.Services.AddMvc().AddApplicationPart(assembly).AddControllersAsServices();
            AddDbProvider(builder.Services, builder.Configuration["DbProvider"], builder.Configuration.GetConnectionString("BlazModular"));
            builder.Services.AddScoped<IModuleBusiness, ModuleBusiness>();
            builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
            return builder;
        }

        public static void MigrateBlazModularDatabase(string dbProvider, string connectionString)
        {
            UpgradeEngine upgradeEngine;

            if (string.IsNullOrEmpty(dbProvider))
                throw new ArgumentNullException(nameof(dbProvider));

            if (dbProvider == "SqlServer")
            {
                upgradeEngine = DeployChanges.To
                                .SqlDatabase(connectionString)
                                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                .LogToConsole()
                                .Build();
            }
            else if (dbProvider == "Sqlite")
            {
                upgradeEngine = DeployChanges.To
                                .SQLiteDatabase(connectionString)
                                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                .LogToConsole()
                                .Build();
            }
            else
            {
                throw new ArgumentException($"Provider not supported: {dbProvider} -- Supported provider is: SqlServer / Sqlite");
            }

            upgradeEngine.PerformUpgrade();
        }

        private static void AddDbProvider(IServiceCollection services, string dbProvider, string connectionString)
        {
            if (string.IsNullOrEmpty(dbProvider))
                throw new ArgumentNullException(nameof(dbProvider));

            if (dbProvider == "SqlServer")
            {
                services.AddDbContext<BlazModularDbContext>(options =>
                {
                    options.UseSqlServer(connectionString, sqlServer =>
                    {
                        sqlServer.EnableRetryOnFailure(3);
                        sqlServer.MigrationsAssembly("BlazModular");
                    });
                });

            }
            else if (dbProvider == "Sqlite")
            {
                services.AddDbContext<BlazModularDbContext>(options =>
                {
                    options.UseSqlite(connectionString, sqlite =>
                    {
                        sqlite.MigrationsAssembly("BlazModular");
                    });
                });
            }
            else
            {
                throw new ArgumentException($"Provider not supported: {dbProvider} -- Supported provider is: SqlServer / Sqlite");
            }

            MigrateBlazModularDatabase(dbProvider, connectionString);
        }
    }
}
