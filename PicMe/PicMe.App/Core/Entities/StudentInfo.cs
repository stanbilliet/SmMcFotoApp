using Newtonsoft.Json;

namespace PicMe.App.Core.Entities
{
    public class StudentInfo
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("givenName")]
        public string GivenName { get; set; }
        [JsonProperty("familyName")]
        public string FamilyName { get; set; }
        [JsonProperty("internalNumber")]
        public string InternalNumber { get; set; }
        [JsonProperty("classCode")]
        public string ClassCode { get; set; }
        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; } = string.Empty;
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }
        public bool IsTakingPicture { get; set; } = false;


    }
}
