using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
