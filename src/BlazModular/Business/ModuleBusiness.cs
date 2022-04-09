using BaGet.Protocol;
using BaGet.Protocol.Models;
using BlazModular.Contracts.Response;
using BlazModular.Entities;
using BlazModular.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using System.IO.Compression;

namespace BlazModular.Business
{
    public class ModuleBusiness : IModuleBusiness
    {
        private const string CachedModuleKey = "cached_modules";
        private readonly string baseRepositoryUri;
        private readonly NuGetClient client;
        private readonly ILogger<ModuleBusiness> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IModuleRepository moduleRepository;

        public ModuleBusiness(IConfiguration configuration, ILogger<ModuleBusiness> logger, IMemoryCache memoryCache, IModuleRepository moduleRepository)
        {
            this.baseRepositoryUri = configuration["BaseRepositoryUri"];
            this.client = new NuGetClient(baseRepositoryUri);
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.moduleRepository = moduleRepository;
        }

        private async Task RefreshModulesAsync()
        {
            memoryCache.Remove(CachedModuleKey);
            await GetModulesAsync();
        }

        private async Task<Module[]> GetModulesAsync()
        {
            var modules = new Module[0];
            if (!memoryCache.TryGetValue(CachedModuleKey, out modules))
            {
                modules = await moduleRepository.QueryModulesAsync();
                memoryCache.Set(CachedModuleKey, modules);
            }

            return modules;
        }

        public async Task<LoadedModule[]> GetLoadedAndActivatedModules(string? path = null)
        {
            var dbModules = await GetModulesAsync();
            return dbModules.Where(m => m.Activated)
                .Select(s => new LoadedModule()
                {
                    Name = s.PackageId,
                    Assembly = s.RawAssembly,
                }).ToArray();
        }

        public async Task<LoadedModule[]> GetLoadedAndInstalledModules(string? path = null)
        {
            var dbModules = await GetModulesAsync();
            return dbModules.Select(s => new LoadedModule()
            {
                Name = s.PackageId,
                Assembly = s.RawAssembly,
            }).ToArray();
        }

        public async Task<SearchResult[]> GetModulesAsync(bool allowBetaChannel = false, bool allowAlphaChannel = false)
        {
            var packages = await client.SearchAsync(string.Empty, 0, 1000, allowAlphaChannel);
            var response = packages.ToArray();

            if (!allowBetaChannel)
            {
                response = response.Where(p => p.Versions.Any(v => !v.Version.Contains("beta"))).ToArray();
            }

            return response.OrderBy(o => o.PackageId).ToArray();
        }

        public async Task<bool> InstallModuleAsync(string packageId, string version)
        {
            try
            {
                string packageFullName = $"{packageId}-{version}.nupkg";
                NuGetVersion packageVersion = new NuGetVersion(version);

                using (Stream packageStream = await client.DownloadPackageAsync(packageId, packageVersion))
                {
                    logger.LogInformation($"Downloaded package {packageId} {packageVersion}");
                    CopyStream(packageStream, packageFullName);
                }

                ZipFile.ExtractToDirectory(packageFullName, $"Tmp/{packageId}", true);

                var assemblyPath = $"Tmp/{packageId}/lib/net7.0/{packageId}.dll";
                var rawDll = File.ReadAllBytes(assemblyPath);

                var installModule = new Module()
                {
                    PackageId = packageId,
                    RawAssembly = rawDll,
                    ActivatedVersion = version
                };

                var insertResult = await moduleRepository.InsertModuleAsync(installModule);
                if (insertResult > 0)
                {
                    await RefreshModulesAsync();
                }

                return insertResult > 0;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on install module.");
                return false;
            }
        }

        private void CopyStream(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}
