using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;
using CyberHeaven.Views;
using CyberHeaven.Models;
using CyberHeaven.Services;
using Microsoft.EntityFrameworkCore;
using CyberHeaven.Repositories;

namespace CyberHeaven.ViewModels
{
   
    public class RegisterViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AppDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LocalizationService _localization;
        public LocalizationService Localization { get; }
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
        public ICommand OpenAuthCommand { get; }
        public ICommand OpenMainCommand { get; }
        public ICommand ExitCommand => new RelayCommand(_ =>
        {
            // Дополнительные действия перед выходом
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Сохранение данных, если необходимо
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка");
                }

                // Закрытие приложения
                Application.Current.Shutdown();
            }
        });
        // Properties
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); ValidateFirstName(); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); ValidateLastName(); }
        }

        private string _selectedPhoneCode;
        public string SelectedPhoneCode
        {
            get => _selectedPhoneCode;
            set { _selectedPhoneCode = value; OnPropertyChanged(); }
        }
        public List<string> PhoneCodes { get; } = new List<string>
{
    "+375 (29)",
    "+375 (33)",
    "+375 (25)"
};

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); ValidatePhone(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); ValidateEmail();
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); ValidateUsername(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); ValidatePassword();
                if (!string.IsNullOrEmpty(ConfirmPassword))
                    ValidateConfirmPassword();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); ValidateConfirmPassword(); }
        }

        // Error messages
        private string _firstNameError;
        public string FirstNameError
        {
            get => _firstNameError;
            set { _firstNameError = value; OnPropertyChanged(); }
        }

        private string _lastNameError;
        public string LastNameError
        {
            get => _lastNameError;
            set { _lastNameError = value; OnPropertyChanged(); }
        }

        private string _phoneError;
        public string PhoneError
        {
            get => _phoneError;
            set { _phoneError = value; OnPropertyChanged(); }
        }

        private string _emailError;
        public string EmailError
        {
            get => _emailError;
            set { _emailError = value; OnPropertyChanged(); }
        }

        private string _usernameError;
        public string UsernameError
        {
            get => _usernameError;
            set { _usernameError = value; OnPropertyChanged(); }
        }

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set { _passwordError = value; OnPropertyChanged(); }
        }

        private string _confirmPasswordError;
        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set { _confirmPasswordError = value; OnPropertyChanged(); }
        }

        // Success message
        private bool _isSuccess;
        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                _isSuccess = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowSuccessElements));
                OnPropertyChanged(nameof(ShowRegisterElements));
            }
        }

        public string SuccessMessage => Localization["RegistrationSuccessful"];
        public bool ShowSuccessElements => IsSuccess;
        public bool ShowRegisterElements => !IsSuccess;

        // Commands
        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }


        public RegisterViewModel(INavigationService navigation, IUnitOfWork unitOfWork)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _dbContext = new AppDbContext();
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _localization = new LocalizationService();
            IsDarkTheme = AppSettings.IsDarkTheme;
            // Подписываемся на изменения языка
            _localization.PropertyChanged += (s, e) =>
            {
                UpdateCurrentErrorMessages();
                // Принудительно обновляем все свойства, зависящие от локализации
                OnPropertyChanged(nameof(SuccessMessage));

            };

            Localization = _localization;
            Localization.SetLanguage(AppSettings.CurrentLanguage);
            RegisterCommand = new RelayCommand(Register);
            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            NavigateToLoginCommand = new RelayCommand(_ => _navigation.NavigateTo<AuthWindow>());
            SwitchToRussianCommand = new RelayCommand(_ => {
                Localization.SetLanguage("ru");
                AppSettings.CurrentLanguage = "ru";
            });
            SwitchToEnglishCommand = new RelayCommand(_ => {
                Localization.SetLanguage("en");
                AppSettings.CurrentLanguage = "en";
            });

        }
        public ICommand SwitchToRussianCommand { get; }
        public ICommand SwitchToEnglishCommand { get; }
        private void UpdateCurrentErrorMessages()
        {
            // Обновляем только существующие ошибки
            if (FirstNameError != null)
                FirstNameError = GetFirstNameErrorText();

            if (LastNameError != null)
                LastNameError = GetLastNameErrorText();

            if (PhoneError != null)
                PhoneError = GetPhoneErrorText();

            if (EmailError != null)
                EmailError = GetEmailErrorText();

            if (UsernameError != null)
                UsernameError = GetUsernameErrorText();

            if (PasswordError != null)
                PasswordError = GetPasswordErrorText();

            if (ConfirmPasswordError != null)
                ConfirmPasswordError = GetConfirmPasswordErrorText();
        }
        private bool IsUsernameExists(string username)
        {
            return _unitOfWork.Users.IsUsernameExists(username);
        }

        private bool SaveUser(User user)
        {
            try
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _unitOfWork.Users.Add(user);
                _unitOfWork.Complete();
                return true;
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return false;
            }
        }
        private void Register(object parameter)
        {
            if (!ValidateAllInputs())
                return;

            var user = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = $"{SelectedPhoneCode} {PhoneNumber}",
                Email = Email,
                Username = Username,
                Password = Password, // В реальном проекте пароль должен быть хеширован!
                Role = "user"
            };

            if (SaveUser(user))
            {
                IsSuccess = true;
            }
        }

        private string GetFirstNameErrorText()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return Localization["FirstNameError"];
            if (FirstName.Length < 2 || FirstName.Length > 50) return Localization["FirstNameLengthError"];
            if (!Regex.IsMatch(FirstName, @"^[\p{L}-]+$")) return Localization["FirstNameNoDigitsError"];
            return null;
        }

        private string GetLastNameErrorText()
        {
            if (string.IsNullOrWhiteSpace(LastName)) return Localization["LastNameError"];
            if (LastName.Length < 2 || LastName.Length > 50) return Localization["LastNameLengthError"];
            if (!Regex.IsMatch(LastName, @"^[\p{L}-]+$")) return Localization["LastNameNoDigitsError"];
            return null;
        }

        private string GetPhoneErrorText()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber)) return Localization["PhoneError"];
            if (!Regex.IsMatch(PhoneNumber, @"^\d{7}$")) return Localization["PhoneFormatError"];
            return null;
        }

        private string GetEmailErrorText()
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (string.IsNullOrWhiteSpace(Email)) return Localization["EmailRequiredError"];
            if (!emailRegex.IsMatch(Email)) return Localization["InvalidEmailError"];
            return null;
        }

        private string GetUsernameErrorText()
        {
            if (string.IsNullOrWhiteSpace(Username)) return Localization["UsernameRequiredError"];
            if (Username.Length < 2 || Username.Length > 20) return Localization["UsernameLengthError"];
            if (IsUsernameExists(Username)) return Localization["UsernameExistsError"];
            return null;
        }

        private string GetPasswordErrorText()
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
            if (string.IsNullOrWhiteSpace(Password)) return Localization["PasswordRequiredError"];
            if (Password.Length < 6) return Localization["PasswordTooShortError"];
            if (!passwordRegex.IsMatch(Password)) return Localization["PasswordComplexityError"];
            return null;
        }

        private string GetConfirmPasswordErrorText()
        {
            if (string.IsNullOrWhiteSpace(ConfirmPassword)) return Localization["ConfirmPasswordRequiredError"];
            if (Password != ConfirmPassword) return Localization["PasswordMismatchError"];
            return null;
        }

        // Методы валидации теперь используют новые методы получения текста ошибок
        private void ValidateFirstName()
        {
            FirstNameError = GetFirstNameErrorText();
            OnPropertyChanged(nameof(FirstNameError));
        }

        private void ValidateLastName()
        {
            LastNameError = GetLastNameErrorText();
            OnPropertyChanged(nameof(LastNameError));
        }

        private void ValidatePhone()
        {
            PhoneError = GetPhoneErrorText();
            OnPropertyChanged(nameof(PhoneError));
        }

        private void ValidateEmail()
        {
            EmailError = GetEmailErrorText();
            OnPropertyChanged(nameof(EmailError));
        }

        private void ValidateUsername()
        {
            UsernameError = GetUsernameErrorText();
            OnPropertyChanged(nameof(UsernameError));
        }

        private void ValidatePassword()
        {
            PasswordError = GetPasswordErrorText();
            OnPropertyChanged(nameof(PasswordError));
        }

        private void ValidateConfirmPassword()
        {
            ConfirmPasswordError = GetConfirmPasswordErrorText();
            OnPropertyChanged(nameof(ConfirmPasswordError));
        }
        private bool ValidateAllInputs()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidatePhone();
            ValidateEmail();
            ValidateUsername();
            ValidatePassword();
            ValidateConfirmPassword();

            return string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(PhoneError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(UsernameError) &&
                   string.IsNullOrEmpty(PasswordError) &&
                   string.IsNullOrEmpty(ConfirmPasswordError);
        }


    }

   
}