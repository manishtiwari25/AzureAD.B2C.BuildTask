using Newtonsoft.Json;

namespace AzureAD.B2C.BuildTask.Modles
{
    [JsonObject]
    public class KeySet
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
