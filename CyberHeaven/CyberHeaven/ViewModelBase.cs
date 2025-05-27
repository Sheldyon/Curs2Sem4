using System.ComponentModel;
using System.Runtime.CompilerServices;
using CyberHeaven.Services;

namespace CyberHeaven.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected LocalizationService _localization = new LocalizationService();
        public LocalizationService Localization => _localization;

        private bool _isDarkTheme = true;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    if (_isDarkTheme)
                    {

                        ThemeManager.ApplyDarkTheme();
                    }
                    else
                        ThemeManager.ApplyLightTheme();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}