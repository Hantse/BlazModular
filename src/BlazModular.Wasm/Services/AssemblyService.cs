using BlazModular.Wasm.Interfaces;
using System.Reflection;

namespace BlazModular.Wasm.Services
{
    public static class AssemblyService
    {
        private static List<Assembly> lazyLoadedAssemblies = new List<Assembly>()
        {
            typeof(BlazModular.Wasm.Bootstrap).Assembly
        };

        public static List<Assembly> GetAssemblies() => lazyLoadedAssemblies;
    }
}
