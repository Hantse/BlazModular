using BlazModular.Wasm.Interfaces;
using BlazModular.Wasm.Services;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace BlazModular.Wasm.Shared
{
    public partial class ModuleContainer : ComponentBase
    {
        [Parameter]
        public EventCallback<List<Assembly>> AssembliesLoadedChanged { get; set; }

        [Inject]
        public IModuleService ModuleService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //NavigationManager.LocationChanged += NavigationManager_LocationChanged;

            //try
            //{
            //    var modulesLoaded = await ModuleService.GetInstalledModules();
            //    foreach (var module in modulesLoaded)
            //    {
            //        Console.WriteLine($"Load module : {module.Name}");
            //        var assembly = Assembly.Load(module.Assembly);
            //        AssemblyService.GetAssemblies().Add(assembly);
            //    }

            //    await UpdateLoadedAssemblies();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Cannot load assembly - {e.Message}");
            //}
        }

        private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            Console.WriteLine("New Module To Load ?");
        }

        async Task UpdateLoadedAssemblies()
        {
            await AssembliesLoadedChanged.InvokeAsync(AssemblyService.GetAssemblies());
        }
    }
}
