using CyberHeaven.Services;
using CyberHeaven.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CyberHeaven.Views
{
    /// <summary>
    /// Логика взаимодействия для ReviewsWindow.xaml
    /// </summary>
    public partial class ReviewsWindow : Window
    {
        public ReviewsWindow()
        {
            INavigationService navigationService = new NavigationService();
            DataContext = new ReviewWindowViewModel(navigationService);
            ThemeManager.ApplyDarkTheme();
            Loaded += ReviewsWindow_Loaded;
            InitializeComponent();

            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; // Применяем к окну для теста
            }
        }
        private void ReviewsWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var storyboard = (Storyboard)FindResource("LogoAnimation");
            storyboard.Begin(CyberText);
            storyboard.Begin(HeavenText);
        }
    }
}
