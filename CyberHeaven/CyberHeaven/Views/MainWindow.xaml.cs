using CyberHeaven.Services;
using CyberHeaven.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
namespace CyberHeaven.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            INavigationService navigationService = new NavigationService();
            DataContext = new MainWindowViewModel(navigationService);
            ThemeManager.ApplyDarkTheme();
            InitializeComponent();

           
            Loaded += MainWindow_Loaded;
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; // Применяем к окну для теста
            }
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var storyboard = (Storyboard)FindResource("LogoAnimation");
            storyboard.Begin(CyberText);
            storyboard.Begin(HeavenText);
        }


    }
}