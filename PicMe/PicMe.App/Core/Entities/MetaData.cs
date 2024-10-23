using Newtonsoft.Json;

namespace PicMe.App.Core.Entities
{
    public class MetaData
    {
        [JsonProperty("smsc.internalNumber")]
        public string InternalNumber { get; set; }
    }
}
