using BaGet.Protocol.Models;
using BlazModular.Wasm.Contracts.Response;

namespace BlazModular.Wasm.Interfaces
{
    public interface IModuleService
    {
        Task<LoadedModule[]> GetInstalledModules(string? path = null);
        Task<SearchResult[]> QueryModulesAsync(bool allowAlphaChannel = false);
        Task<bool> InstallModuleAsync(string packageId, string version);
    }
}
