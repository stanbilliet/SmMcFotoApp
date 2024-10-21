using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.App.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public string name;

        public async Task ChangeNameBySchoolName()
        {
            var schoolName = await SecureStorage.GetAsync("SchoolName");
            Name = $"Hallo {schoolName}!";
        }
    }
}
