using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UP01_223_Nikolaeva.Model;

namespace UP01_223_Nikolaeva
{
    public static class ThemeChanger
    {
        private const string LightPath = "/Themes/DictionaryStyle.xaml";
        private const string BlackPath = "/Themes/DictionaryStyleBlack.xaml";

        public static void ApplyTheme(bool isLight, bool saveToDb = false)
        {
            var dicts = Application.Current.Resources.MergedDictionaries;
            dicts.Clear();
            dicts.Add(new ResourceDictionary
            {
                Source = new Uri(isLight ? LightPath : BlackPath, UriKind.Relative)
            });

            if (saveToDb)
            {
                UserData.CurrentUser.IsLightTheme = isLight;
                Core.context.SaveChanges();
            }
        }

        public static void ChangeTheme()
        {

            bool newIsLight = !UserData.CurrentUser.IsLightTheme;
            ApplyTheme(newIsLight, true);
        }
    }
}

