using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CyberHeaven.ViewModels
{
    public class UserCabinetViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AppDbContext _dbContext;
        private User _currentUser;
        private bool _isEditMode;
        private string _tempUsername;
        private string _tempEmail;
        private string _tempPhone;
        private string _usernameError;
        private string _emailError;
        private string _phoneError;
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
                    {
                        ThemeManager.ApplyDarkTheme();

                    }
                    else
                    {
                        ThemeManager.ApplyLightTheme();

                    }

                }
            }
        }
        private ObservableCollection<Booking> _bookings;
        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
                OnPropertyChanged(nameof(HasBookings)); 
            }
        }

        public bool HasBookings => Bookings?.Count > 0;

        public ICommand CancelBookingCommand { get; }
        public ICommand ToggleThemeCommand => new RelayCommand(_ =>
        {
            IsDarkTheme = !IsDarkTheme;
        });
        public ICommand SwitchToRussianCommand => new RelayCommand(_ =>
        {
            Localization.SetLanguage("ru");
            AppSettings.CurrentLanguage = "ru";
        });

        public ICommand SwitchToEnglishCommand => new RelayCommand(_ =>
        {
            Localization.SetLanguage("en");
            AppSettings.CurrentLanguage = "en";
        });

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public string TempUsername
        {
            get => _tempUsername;
            set
            {
                SetProperty(ref _tempUsername, value);
                ValidateUsername();
            }
        }

        public string TempEmail
        {
            get => _tempEmail;
            set
            {
                SetProperty(ref _tempEmail, value);
                ValidateEmail();
            }
        }

        public string TempPhone
        {
            get => _tempPhone;
            set
            {
                SetProperty(ref _tempPhone, value);
                ValidatePhone();
            }
        }

        public string UsernameError
        {
            get => _usernameError;
            set => SetProperty(ref _usernameError, value);
        }

        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        public string PhoneError
        {
            get => _phoneError;
            set => SetProperty(ref _phoneError, value);
        }

        public string UserImagePath { get; set; }

        public ICommand EditProfileCommand { get; }
        public ICommand SaveProfileCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand OpenMainCommand { get; }
        public ICommand ChangeAvatarCommand { get; }
        public ICommand OpenBronCommand { get; }
        public ICommand OpenReviewCommand { get; }
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

        public UserCabinetViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            _dbContext = new AppDbContext();

            CurrentUser = Application.Current.Properties["CurrentUser"] as User;
           
                LoadBookings();
            
            UserImagePath = !string.IsNullOrEmpty(CurrentUser?.AvatarPath)
                ? CurrentUser.AvatarPath
                : "pack://application:,,,/images/UserWithoutLogIn.png";

            EditProfileCommand = new RelayCommand(_ =>
            {
                IsEditMode = true;
                TempUsername = CurrentUser.Username;
                TempEmail = CurrentUser.Email;
                TempPhone = CurrentUser.Phone;
            });

            SaveProfileCommand = new RelayCommand(_ => SaveProfile());
            CancelEditCommand = new RelayCommand(_ => IsEditMode = false);
            LogoutCommand = new RelayCommand(_ => Logout());
            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            ChangeAvatarCommand = new RelayCommand(_ => ChangeAvatar());
            OpenBronCommand = new RelayCommand(_ => _navigation.NavigateTo<BronWindow>());
            OpenReviewCommand = new RelayCommand(_ => _navigation.NavigateTo<ReviewsWindow>());
            CancelBookingCommand = new RelayCommand(param =>
            {
                if (param is int bookingId)
                {
                    CancelBooking(bookingId);
                }
            });


            Localization.SetLanguage(AppSettings.CurrentLanguage);
            IsDarkTheme = AppSettings.IsDarkTheme;

        }
        private void LoadBookings()
        {
            if (CurrentUser == null) return;

            try
            {
                var bookings = _dbContext.Bookings
                    .Include(b => b.Place) 
                    .Where(b => b.UserId == CurrentUser.Id)
                    .OrderByDescending(b => b.SelectedDate)
                    .ThenByDescending(b => b.StartTime)
                    .ToList();

                Bookings = new ObservableCollection<Booking>(bookings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки бронирований: {ex.Message}");
            }
        }
        private void CancelBooking(int bookingId)
        {
            var booking = Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return;

            if (MessageBox.Show("Вы уверены, что хотите отменить бронирование?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _dbContext.Bookings.Remove(booking);
                    _dbContext.SaveChanges();

                    Bookings.Remove(booking);

                    MessageBox.Show("Бронирование успешно отменено", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отмене бронирования: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(TempUsername))
            {
                UsernameError = "Никнейм не может быть пустым";
                return;
            }

            if (TempUsername.Length < 2 || TempUsername.Length > 50)
            {
                UsernameError = "Никнейм должен быть от 2 до 50 символов";
                return;
            }

            UsernameError = string.Empty;
        }

        private void ValidateEmail()
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (string.IsNullOrWhiteSpace(TempEmail))
            {
                EmailError = "Email не может быть пустым";
                return;
            }

            try
            {
                if (!emailRegex.IsMatch(TempEmail))
                {
                    EmailError = "Некорректный формат email";
                    return;
                }
            }
            catch
            {
                EmailError = "Некорректный формат email";
                return;
            }

            EmailError = string.Empty;
        }

        private void ValidatePhone()
        {
            if (string.IsNullOrEmpty(TempPhone))
            {
                PhoneError = string.Empty;
                return;
            }

            var regex = new Regex(@"^\+375\((29|33|25|)\)\d{7}$");
            if (!regex.IsMatch(TempPhone))
            {
                PhoneError = "Формат: +375(29/33/25/)1234567";
                return;
            }

            PhoneError = string.Empty;
        }

        private bool IsFormValid()
        {
            ValidateUsername();
            ValidateEmail();
            ValidatePhone();

            return string.IsNullOrEmpty(UsernameError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(PhoneError);
        }

        private void SaveProfile()
        {
            if (!IsFormValid())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                CurrentUser.Username = TempUsername;
                CurrentUser.Email = TempEmail;
                CurrentUser.Phone = TempPhone;

                _dbContext.Users.Update(CurrentUser);
                _dbContext.SaveChanges();

                Application.Current.Properties["CurrentUser"] = CurrentUser;

                IsEditMode = false;
                MessageBox.Show("Ваши данные изменены! Перезайдите на страницу повторно", "Успех",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Application.Current.Properties["UserImagePath"] = null;
            _navigation.NavigateTo<MainWindow>();
        }

        private void ChangeAvatar()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите изображение для аватарки"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    UserImagePath = openFileDialog.FileName;
                    CurrentUser.AvatarPath = UserImagePath;

                    _dbContext.Users.Update(CurrentUser);
                    _dbContext.SaveChanges();

                    Application.Current.Properties["UserImagePath"] = UserImagePath;
                    Application.Current.Properties["CurrentUser"] = CurrentUser;

                    OnPropertyChanged(nameof(UserImagePath));
                    MessageBox.Show("Аватарка успешно изменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка при изменении аватарки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}