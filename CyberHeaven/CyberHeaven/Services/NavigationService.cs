using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CyberHeaven.ViewModels;

namespace CyberHeaven.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : Window, new();
        void CloseCurrent();

    }

    public class NavigationService : INavigationService
    {
        public void NavigateTo<T>() where T : Window, new()
        {
            var currentWindow = Application.Current.MainWindow;

            var newWindow = new T();
            if (newWindow.DataContext is ViewModelBase vm)
            {
                vm.IsDarkTheme = AppSettings.IsDarkTheme;
                vm.Localization.SetLanguage(AppSettings.CurrentLanguage);

            }
            Application.Current.MainWindow = newWindow;

            newWindow.Show();

            // Закрываем предыдущее окно после успешного открытия нового
            currentWindow?.Close();
        }

        public void CloseCurrent()
        {
            Application.Current.MainWindow?.Close();
        }


    }
}
