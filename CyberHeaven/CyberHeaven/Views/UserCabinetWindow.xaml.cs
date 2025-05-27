using CyberHeaven.Services;
using CyberHeaven.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace CyberHeaven.Views
{
    public partial class UserCabinetWindow : Window
    {
        public UserCabinetWindow()
        {
            ThemeManager.ApplyDarkTheme();
            INavigationService navigationService = new NavigationService();
            DataContext = new UserCabinetViewModel(navigationService);
           
            InitializeComponent();
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; // Применяем к окну для теста
            }

        }

    }
}