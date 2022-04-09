using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlazModular.Wasm;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SampleApp.Client;

public class Program
{
    private static WebAssemblyHostBuilder app;

    public static async Task Main(string[] args)
    {
        app = WebAssemblyHostBuilder.CreateDefault(args);
        app.RootComponents.Add<App>("#app");
        app.RootComponents.Add<HeadOutlet>("head::after");
        app.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(app.HostEnvironment.BaseAddress) });
        await app.Services.RegisterBlazModular(app);
        app.ConfigureContainer(new AutofacServiceProviderFactory(ConfigureContainer));

        var buildedApp = app.Build();
        await buildedApp.RunAsync();
    }

    static void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.Populate(app.Services);
    }
}
