using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven
{
    public static class AppSettings
    {
        public static bool IsDarkTheme { get; set; } = true;
        public static string CurrentLanguage { get; set; } = "ru";
        public static int CurrentUserId
        {
            get => int.Parse(ConfigurationManager.AppSettings["CurrentUserId"] ?? "0");
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["CurrentUserId"].Value = value.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
