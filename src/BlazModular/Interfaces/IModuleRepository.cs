using BlazModular.Entities;

namespace BlazModular.Interfaces
{
    public interface IModuleRepository
    {
        Task<Module[]> QueryModulesAsync();
        Task<int> InsertModuleAsync(Module entity);
        Task<int> UpdateModuleAsync(Module entity);
    }
}
