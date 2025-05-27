using System.Windows;

namespace CyberHeaven.Services
{
    public static class ThemeManager
    {
        public static void ApplyDarkTheme()
        {
            ApplyTheme(new Uri("/CyberHeaven;component/Resources/DarkTheme.xaml", UriKind.Relative));
        }

        public static void ApplyLightTheme()
        {
            ApplyTheme(new Uri("/CyberHeaven;component/Resources/LightTheme.xaml", UriKind.Relative));
        }

        private static void ApplyTheme(Uri themeUri)
        {
            // 1. Загружаем общие ресурсы
            

            // 2. Загружаем выбранную тему
            var themeDict = new ResourceDictionary { Source = themeUri };

            // 3. Загружаем локализацию
            var stringsDict = new ResourceDictionary
            {
                Source = new Uri("/CyberHeaven;component/Resources/Strings.xaml", UriKind.Relative)
            };

            // Очищаем и добавляем новые ресурсы
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
            Application.Current.Resources.MergedDictionaries.Add(stringsDict);
        }
    }
}