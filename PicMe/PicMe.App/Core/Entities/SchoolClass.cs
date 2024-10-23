using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace PicMe.Core.Entities
{
    public partial class SchoolClass : ObservableObject
    {
        [JsonProperty("classCode")]
        public string ClassCode { get; set; }
        public string ClassProfilePicture { get; set; }
        [ObservableProperty]
        public ImageSource userImage;
    }
}
