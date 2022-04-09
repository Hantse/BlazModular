using BlazModular.Entities;
using BlazModular.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazModular.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly BlazModularDbContext blazModularDbContext;

        public ModuleRepository(BlazModularDbContext blazModularDbContext)
        {
            this.blazModularDbContext = blazModularDbContext;
        }

        public Task<Module[]> QueryModulesAsync()
        {
            return blazModularDbContext.Module.ToArrayAsync();
        }

        public Task<int> InsertModuleAsync(Module entity)
        {
            blazModularDbContext.Module.Add(entity);
            return blazModularDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateModuleAsync(Module entity)
        {
            blazModularDbContext.Entry(entity).State = EntityState.Modified;
            return blazModularDbContext.SaveChangesAsync();
        }
    }
}
