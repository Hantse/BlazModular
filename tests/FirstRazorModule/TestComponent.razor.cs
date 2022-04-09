using Microsoft.AspNetCore.Components;

namespace FirstRazorModule
{
    public partial class TestComponent : ComponentBase
    {
        [Inject]
        public ITestService TestService { get; set; }
    }
}
