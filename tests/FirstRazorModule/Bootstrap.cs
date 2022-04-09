using Blazorise;
using Blazorise.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace FirstRazorModule
{
    public class Bootstrap
    {
        public void Register(IServiceCollection services)
        {
            services.AddAntDesign();
            services.AddBlazorise().AddEmptyProviders();
            services.AddScoped<ITestService, TestService>();
        }
    }
}
