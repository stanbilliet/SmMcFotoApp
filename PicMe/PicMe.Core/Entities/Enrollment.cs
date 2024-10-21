using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Entities
{
    public class Enrollment
    {
        [JsonProperty("user")]
        public Student Students { get; set; }
        [JsonProperty("class")]
        public SchoolClass SchoolClasses { get; set; }
    }
}
