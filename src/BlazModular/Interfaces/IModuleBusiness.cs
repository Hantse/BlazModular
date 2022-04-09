using BaGet.Protocol.Models;
using BlazModular.Contracts.Response;
using System.Threading.Tasks;

namespace BlazModular.Interfaces
{
    public interface IModuleBusiness
    {
        Task<LoadedModule[]> GetLoadedAndInstalledModules(string? path = null);
        Task<SearchResult[]> GetModulesAsync(bool allowBetaChannel = false, bool allowAlphaChannel = false);
        Task<bool> InstallModuleAsync(string packageId, string version);
    }
}
