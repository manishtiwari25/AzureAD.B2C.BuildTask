using Newtonsoft.Json;

namespace AzureAD.B2C.BuildTask.Modles
{
    [JsonObject]
    public class GenerateKey
    {
        [JsonProperty("use")]
        public string Use { get; set; }
        [JsonProperty("kty")]
        public string Kty { get; set; }
    }
}
