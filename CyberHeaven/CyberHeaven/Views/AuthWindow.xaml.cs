using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CyberHeaven.Controls;
using CyberHeaven.Services;
using CyberHeaven.ViewModels;

namespace CyberHeaven.Views
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            INavigationService navigationService = new NavigationService();
            DataContext = new AuthViewModel(navigationService);
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; 
            }
            InitializeComponent();

        }
        private void OpenHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Логика открытия справки
            MessageBox.Show("Help information will be displayed here.",
                          "Help",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void RefreshData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Логика обновления данных
            if (DataContext is AuthViewModel viewModel)
            {
                viewModel.Username = "";
                viewModel.Password = "";
                viewModel.AuthError = "";
            }
        }
        private void PasswordInputControl_PasswordChanged(object sender, EventArgs e)
        {
            if (DataContext is AuthViewModel viewModel)
            {
                viewModel.Password = ((PasswordInputControl)sender).Password;
            }
        }
        private void PasswordInputControl_PasswordStrengthChecked(object sender, RoutedEventArgs e)
        {
            // This demonstrates bubble routing - event bubbles up from the control
            MessageBox.Show("Password strength checked (Bubbling event)");
        }

        private void PasswordInputControl_PasswordVisibilityToggling(object sender, RoutedEventArgs e)
        {
            // This demonstrates tunnel routing - event tunnels down to the control
            MessageBox.Show("About to toggle password visibility (Tunneling event)");
        }

        private void PasswordInputControl_PasswordVisibilityToggled(object sender, RoutedEventArgs e)
        {
            // This demonstrates direct routing - event goes directly to handler
            MessageBox.Show("Password visibility toggled (Direct event)");
        }
    }
}