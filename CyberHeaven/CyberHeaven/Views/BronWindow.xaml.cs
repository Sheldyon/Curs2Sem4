using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CyberHeaven.Services;
using CyberHeaven.ViewModels;

namespace CyberHeaven.Views
{
    public partial class BronWindow : Window
    {
        public BronWindow()
        {
            ThemeManager.ApplyDarkTheme();
            InitializeComponent();

            // Инициализация ViewModel с NavigationService
            var navigationService = new NavigationService();
            DataContext = new BronWindowViewModel(navigationService);
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; // Применяем к окну для теста
            }
            Loaded += BronWindow_Loaded;
        }

        private void BronWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Анимация при загрузке (аналогично MainWindow)
            if (CyberText != null && HeavenText != null)
            {
                var storyboard = (Storyboard)FindResource("LogoAnimation");
                storyboard.Begin(CyberText);
                storyboard.Begin(HeavenText);
            }
        }
        private void PromoCodePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is BronWindowViewModel vm)
            {
                vm.PromoCode = PromoCodePasswordBox.Password;
            }
        }
    }
}