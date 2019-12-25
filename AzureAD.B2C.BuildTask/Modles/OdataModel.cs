namespace AzureAD.B2C.BuildTask.Modles
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject]
    public class OdataModel<T>
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public List<T> Values { get; set; }
    }
}
