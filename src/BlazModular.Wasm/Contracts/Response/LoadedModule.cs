using System.Text.Json.Serialization;

namespace BlazModular.Wasm.Contracts.Response
{
    public class LoadedModule
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("assembly")]
        public byte[] Assembly { get; set; }
    }
}
