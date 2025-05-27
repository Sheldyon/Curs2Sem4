
    using System.Windows;
    using System.Windows.Controls;
using System.Windows.Input;
using CyberHeaven.Models;
using CyberHeaven.Repositories;
using CyberHeaven.Services;
using CyberHeaven.ViewModels;
using Microsoft.EntityFrameworkCore;

    namespace CyberHeaven.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            INavigationService navigationService = new NavigationService();
            var dbContext = new AppDbContext();
            var unitOfWork = new UnitOfWork(dbContext);
            DataContext = new RegisterViewModel(navigationService, unitOfWork);
           
            InitializeComponent();
            if (Application.Current.Resources["DefaultCursor"] is Cursor defaultCursor)
            {
                this.Cursor = defaultCursor; // Применяем к окну для теста
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }


    }
}