using Newtonsoft.Json;
using PicMe.Core.Entities;

namespace PicMe.App.Core.Entities
{
    public class Enrollment
    {
        [JsonProperty("user")]
        public Student Students { get; set; }
        [JsonProperty("class")]
        public SchoolClass SchoolClasses { get; set; }
    }
}
