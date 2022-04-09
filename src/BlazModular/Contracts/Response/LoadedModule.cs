namespace BlazModular.Contracts.Response
{
    public class LoadedModule
    {
        public string Name { get; set; }
        public byte[] Assembly { get; set; }
        public string PathLoad { get; set; }
    }
}
