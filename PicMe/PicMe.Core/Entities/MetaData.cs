using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Entities
{
    public class MetaData
    {
        [JsonProperty("smsc.internalNumber")]
        public string InternalNumber { get; set; }
    }
}
