using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CyberHeaven.Services;
using CyberHeaven.ViewModels;

namespace CyberHeaven.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            INavigationService navigationService = new NavigationService();
            DataContext = new AdminDashboardViewModel(navigationService);
            ThemeManager.ApplyDarkTheme();
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; // Применяем к окну для теста
            }
            InitializeComponent();
        }
    }
}
