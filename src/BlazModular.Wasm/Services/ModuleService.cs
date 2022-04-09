using BaGet.Protocol.Models;
using BlazModular.Wasm.Contracts.Response;
using BlazModular.Wasm.Interfaces;
using System.Text.Json;

namespace BlazModular.Wasm.Services
{
    public class ModuleService : IModuleService
    {
        private readonly HttpClient httpClient;

        public ModuleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public ModuleService(string uri)
        {
            this.httpClient = new HttpClient() { BaseAddress = new Uri(uri) };
        }

        public async Task<LoadedModule[]> GetInstalledModules(string? path = null)
        {
            var httpResponse = await httpClient.GetAsync($"api/module/loaded?path={path}");
            if (httpResponse.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<LoadedModule[]>(await httpResponse.Content.ReadAsStringAsync());
            }

            return new LoadedModule[0];
        }

        public async Task<SearchResult[]> QueryModulesAsync(bool allowAlphaChannel = false)
        {
            var httpResponse = await httpClient.GetAsync($"api/module?allowAlphaChannel={allowAlphaChannel}");
            if (httpResponse.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<SearchResult[]>(await httpResponse.Content.ReadAsStringAsync());
            }

            return new SearchResult[0];
        }

        public async Task<bool> InstallModuleAsync(string packageId, string version)
        {
            var httpResponse = await httpClient.GetAsync($"api/module/install?packageName={packageId}&version={version}");
            return httpResponse.IsSuccessStatusCode;
        }
    }
}
