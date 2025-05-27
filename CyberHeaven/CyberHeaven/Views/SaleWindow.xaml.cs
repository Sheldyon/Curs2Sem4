using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CyberHeaven.Services;
using CyberHeaven.ViewModels;

namespace CyberHeaven.Views
{
    public partial class SaleWindow : Window
    {
        public SaleWindow()
        {
            INavigationService navigationService = new NavigationService();
            DataContext = new SaleWindowViewModel(navigationService);
            ThemeManager.ApplyDarkTheme();
            InitializeComponent();
            Loaded += SaleWindow_Loaded;
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor;
            }


        }
        private void SaleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("LogoAnimation");
            storyboard.Begin(CyberText);
            storyboard.Begin(HeavenText);

        }

       
    }
}