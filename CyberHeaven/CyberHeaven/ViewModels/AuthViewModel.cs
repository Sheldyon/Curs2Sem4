using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CyberHeaven.Views;
using CyberHeaven.Models;
using CyberHeaven.Services;
using Microsoft.EntityFrameworkCore;

namespace CyberHeaven.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;
        private List<string> _randomUserImages = new List<string>
        {
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\apex-legendsmain.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\apex-legendsmain.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\apex-legendsmain.png",
        };

        private string GetRandomUserImage()
        {
            Random rnd = new Random();
            return _randomUserImages[rnd.Next(_randomUserImages.Count)];
        }

        private readonly INavigationService _navigation;
        private string _username;
        private string _password;
        private string _usernameError;
        private string _passwordError;
        private string _authError;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ValidateUsername();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ValidatePassword();
            }
        }

        public string UsernameError
        {
            get => _usernameError;
            set
            {
                _usernameError = value;
                OnPropertyChanged();
            }
        }

        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged();
            }
        }

        public string AuthError
        {
            get => _authError;
            set
            {
                _authError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAuthError));
            }
        }

        public bool HasAuthError => !string.IsNullOrEmpty(AuthError);
        public LocalizationService Localization { get; } = new LocalizationService();
        private bool _isDarkTheme = AppSettings.IsDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    AppSettings.IsDarkTheme = value;
                    if (value)
                        ThemeManager.ApplyDarkTheme();
                    else
                        ThemeManager.ApplyLightTheme();
                }
            }
        }

        public ICommand ToggleThemeCommand => new RelayCommand(_ =>
        {
            IsDarkTheme = !IsDarkTheme;
        });
        public ICommand SwitchToRussianCommand => new RelayCommand(_ =>
        {
            Localization.SetLanguage("ru");
            UpdateErrorMessagesLanguage();
            AppSettings.CurrentLanguage = "ru";

        });

        public ICommand SwitchToEnglishCommand => new RelayCommand(_ =>
        {
            Localization.SetLanguage("en");
            UpdateErrorMessagesLanguage();
            AppSettings.CurrentLanguage = "en";
        });
        public ICommand ExitCommand => new RelayCommand(_ =>
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка");
                }

                Application.Current.Shutdown();
            }
        });
        public ICommand OpenMainCommand { get; }
 
        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public AuthViewModel(INavigationService navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _dbContext = new AppDbContext();
            LoginCommand = new RelayCommand(Login);
            NavigateToRegisterCommand = new RelayCommand(_ => _navigation.NavigateTo<RegisterWindow>());
            IsDarkTheme = AppSettings.IsDarkTheme;
            Localization.SetLanguage(AppSettings.CurrentLanguage);
            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
        }

        private void UpdateErrorMessagesLanguage()
        {
            if (!string.IsNullOrEmpty(UsernameError))
                UsernameError = Localization["UsernameRequiredError"];

            if (!string.IsNullOrEmpty(PasswordError))
                PasswordError = Localization["PasswordRequiredError"];

            if (!string.IsNullOrEmpty(AuthError))
            {

                AuthError = Localization["Uncorrectusernameorpassword"];
            }
        }

        private void Login(object parameter)
        {
            AuthError = string.Empty;
            if (!ValidateInputs())
                return;

            try
            {
                if (Username == "admin" && Password == "adminadmin123")
                {
                    var adminUser = new User
                    {
                        FirstName = "Admin",
                        LastName = "Adminov",
                        Email = "admin@cyberheaven.com", 
                        Phone = "+375 (29) 3657910",
                        Username = "admin",
                        Password = BCrypt.Net.BCrypt.HashPassword("adminadmin123"),
                        Role = "admin"
                    };

                    if (!_dbContext.Users.Any(u => u.Username == "admin"))
                    {
                        _dbContext.Users.Add(adminUser);
                        _dbContext.SaveChanges();
                    }

                    Application.Current.Properties["CurrentUser"] = adminUser;
                    Application.Current.Properties["UserImagePath"] = "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\admin_icon.png";
                    _navigation.NavigateTo<MainWindow>();
                    return;
                }

                var user = _dbContext.Users
                    .FirstOrDefault(u => u.Username == Username);

                if (user == null)
                {
                   
                    AuthError = Localization["Uncorrectusernameorpassword"];
                    return;
                }
                if (user.IsBlocked)
                {
                    MessageBox.Show("Этот пользователь заблокирован и не может войти в систему.",
                                   "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
                {
                    AuthError = Localization["Uncorrectusernameorpassword"];
                    return;
                }
                Application.Current.Properties["CurrentUser"] = user;
                Application.Current.Properties["UserImagePath"] = GetRandomUserImage();
                _navigation.NavigateTo<MainWindow>();
            }
            catch (DbUpdateException ex)
            {
                AuthError = Localization["DatabaseError"];
                MessageBox.Show($"Database error: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                AuthError = Localization["AuthError"];
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = Localization["UsernameRequiredError"];
            }
            else
            {
                UsernameError = string.Empty;
            }
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = Localization["PasswordRequiredError"];
            }
            else
            {
                PasswordError = string.Empty;
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            ValidateUsername();
            ValidatePassword();

            if (!string.IsNullOrEmpty(UsernameError) || !string.IsNullOrEmpty(PasswordError))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}