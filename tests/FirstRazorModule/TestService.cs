namespace FirstRazorModule
{
    public interface ITestService
    {
        void Test();
    }

    public class TestService : ITestService
    {
        public void Test()
        {
            Console.WriteLine($"TEST");
        }
    }
}