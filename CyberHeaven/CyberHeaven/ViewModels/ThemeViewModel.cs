using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CyberHeaven.ViewModels
{

    public class ThemeViewModel : INotifyPropertyChanged
    {
        public ICommand SwitchThemeCommand { get; }

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                _isDarkTheme = value;
                OnPropertyChanged();
                ApplyTheme();
            }
        }

        public ThemeViewModel()
        {
            SwitchThemeCommand = new RelayCommand(SwitchTheme);
            // Загружаем текущую тему (по умолчанию светлая)
            IsDarkTheme = false;
        }

        private void SwitchTheme(object parameter)
        {
            IsDarkTheme = !IsDarkTheme;
        }

        private void ApplyTheme()
        {
            var app = Application.Current;

            // Удаляем текущую тему
            var existingTheme = app.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                    (d.Source.OriginalString.Contains("DarkTheme.xaml") ||
                     d.Source.OriginalString.Contains("LightTheme.xaml")));

            if (existingTheme != null)
            {
                app.Resources.MergedDictionaries.Remove(existingTheme);
            }

            // Добавляем новую тему
            var newTheme = new ResourceDictionary
            {
                Source = new Uri(_isDarkTheme
                    ? "D:/ООП/CyberHeaven/CyberHeaven/Resources/DarkTheme.xaml"
                    : "D:/ООП/CyberHeaven/CyberHeaven/Resources/LightTheme.xaml", UriKind.Absolute)
            };

            app.Resources.MergedDictionaries.Add(newTheme);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
