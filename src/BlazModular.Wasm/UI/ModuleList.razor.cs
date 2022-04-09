using BaGet.Protocol.Models;
using BlazModular.Wasm.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazModular.Wasm.UI
{
    public partial class ModuleList : ComponentBase
    {
        [Inject]
        public IModuleService ModuleService { get; set; }

        private SearchResult[] modules;
        private string selectedVersion = "1.1.2";

        protected override async Task OnInitializedAsync()
        {
            modules = await ModuleService.QueryModulesAsync(true);
        }

        private async Task InstallModuleAsync(string packageId)
        {
            await ModuleService.InstallModuleAsync(packageId, selectedVersion);
        }
    }
}
