using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.App.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
		private string selectedLanguage;	

		public string SelectedLanguage
        {
			get 
			{
                return Preferences.Get("appLanguage", "nl");

            }
			set 
			{ 
				if(SetProperty(ref selectedLanguage, value))
				{
                    Console.WriteLine($"SelectedLanguage changed to: {value}");
                    SetCulture(value);
                }
			}
		}

		private void SetCulture(string language)
		{

            var culture = new System.Globalization.CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Preferences.Set("appLanguage", language);

            OnLanguageChanged();
        }

		public virtual void OnLanguageChanged()
		{
        }
	}
}
