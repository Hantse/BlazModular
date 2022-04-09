using BlazModular.Wasm.Interfaces;
using BlazModular.Wasm.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazModular.Wasm
{
    public static class Bootstrap
    {

        public static async Task<IServiceCollection> RegisterBlazModular(this IServiceCollection services, WebAssemblyHostBuilder app)
        {
            services.AddAntDesign();
            services.AddScoped<IModuleService, ModuleService>();
            await services.LoadModuleDependencies(app);
            return services;
        }

        private static async Task LoadModuleDependencies(this IServiceCollection services, WebAssemblyHostBuilder app)
        {
            var moduleService = new ModuleService(app.HostEnvironment.BaseAddress);
            if (moduleService != null)
            {
                var activatedModules = await moduleService.GetInstalledModules();

                foreach (var module in activatedModules)
                {
                    var assembly = Assembly.Load(module.Assembly);
                    object bootstrap = assembly.CreateInstance($"{assembly.GetName().Name}.Bootstrap");
                    AssemblyService.GetAssemblies().Add(assembly);

                    if (bootstrap != null)
                    {
                        ((dynamic)bootstrap).Register(services);
                    }
                }
            }
        }
    }
}
