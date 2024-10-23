using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PicMe.App.Core.Entities;

namespace PicMe.Core.Entities
{
    public partial class Student : ObservableObject
    {
        public string InternalNumber { get; set; }
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("givenName")]
        public string GivenName { get; set; }
        [JsonProperty("familyName")]
        public string FamilyName { get; set; }
        [JsonProperty("metadata")]
        public MetaData MetaData { get; set; }
        public string ProfilePicture { get; set; }
        [ObservableProperty]
        public ImageSource userImage;
    }
}
