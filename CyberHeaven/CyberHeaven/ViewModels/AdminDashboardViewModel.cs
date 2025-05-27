using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CyberHeaven.ViewModels
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        private readonly string _connectionString;
        private bool _isUserEditMode;
        public bool IsUserEditMode
        {
            get => _isUserEditMode;
            set => SetProperty(ref _isUserEditMode, value);
        }

        private string _tempUsername;
        public string TempUsername
        {
            get => _tempUsername;
            set => SetProperty(ref _tempUsername, value);
        }

        private string _tempEmail;
        public string TempEmail
        {
            get => _tempEmail;
            set => SetProperty(ref _tempEmail, value);
        }

        private string _tempPhone;
        public string TempPhone
        {
            get => _tempPhone;
            set => SetProperty(ref _tempPhone, value);
        }

        private readonly INavigationService _navigation;
        private User _currentAdmin;
        private bool _isDarkTheme = AppSettings.IsDarkTheme;
        private ObservableCollection<Booking> _pendingBookings;
        private ObservableCollection<Booking> _activeBookings;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        public const string ApprovedStatus = "Подтверждено";

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

        private ICommand _selectUserCommand;
        public ICommand SelectUserCommand => _selectUserCommand ??= new RelayCommand(SelectUser);
        public ICommand EditUserCommand { get; }
        public ICommand SaveUserChangesCommand { get; }
        public ICommand CancelUserEditCommand { get; }

        private void SelectUser(object parameter)
        {
            if (parameter is User user)
            {
                SelectedUser = user;
                IsUserEditMode = false;
                TempUsername = user.Username;
                TempEmail = user.Email;
                TempPhone = user.Phone;
            }
        }

        public User CurrentAdmin
        {
            get => _currentAdmin;
            set => SetProperty(ref _currentAdmin, value);
        }

        public ObservableCollection<Booking> PendingBookings
        {
            get => _pendingBookings;
            set
            {
                _pendingBookings = value;
                OnPropertyChanged(nameof(PendingBookings));
            }
        }

        public ObservableCollection<Booking> ActiveBookings
        {
            get => _activeBookings;
            set
            {
                _activeBookings = value;
                OnPropertyChanged(nameof(ActiveBookings));
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                OnPropertyChanged(nameof(CanEditUser));
            }
        }

        public bool CanEditUser => SelectedUser != null;

        public ICommand ApproveBookingCommand { get; }
        public ICommand RejectBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }
        public ICommand ToggleThemeCommand { get; }
        public ICommand SwitchToRussianCommand { get; }
        public ICommand SwitchToEnglishCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand OpenMainCommand { get; }
        public ICommand ChangeUserAvatarCommand { get; }
        public ICommand ToggleUserBlockCommand { get; }
        public ICommand CompleteBookingCommand { get; }
        public ICommand ExitCommand => new RelayCommand(_ =>
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        });

        public AdminDashboardViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            _connectionString = ConfigurationManager.ConnectionStrings["CyberHeavenDB"].ConnectionString;

            CurrentAdmin = Application.Current.Properties["CurrentUser"] as User;
            LoadData();

            ToggleThemeCommand = new RelayCommand(_ => IsDarkTheme = !IsDarkTheme);
            SwitchToRussianCommand = new RelayCommand(_ =>
            {
                Localization.SetLanguage("ru");
                AppSettings.CurrentLanguage = "ru";
            });

            SwitchToEnglishCommand = new RelayCommand(_ =>
            {
                Localization.SetLanguage("en");
                AppSettings.CurrentLanguage = "en";
            });

            LogoutCommand = new RelayCommand(_ => Logout());
            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            ApproveBookingCommand = new RelayCommand(param => ApproveBooking((int)param));
            RejectBookingCommand = new RelayCommand(param => RejectBooking((int)param));
            CancelBookingCommand = new RelayCommand(param => CancelBooking((int)param));
            CompleteBookingCommand = new RelayCommand(param => CompleteBooking((int)param));
            EditUserCommand = new RelayCommand(_ => StartUserEdit());
            SaveUserChangesCommand = new RelayCommand(_ => SaveUserChanges());
            CancelUserEditCommand = new RelayCommand(_ => CancelUserEdit());

            ChangeUserAvatarCommand = new RelayCommand(_ => ChangeUserAvatar());
            ToggleUserBlockCommand = new RelayCommand(_ => ToggleUserBlock());

            Localization.SetLanguage(AppSettings.CurrentLanguage);
            IsDarkTheme = AppSettings.IsDarkTheme;
        }

        private void LoadData()
        {
            LoadPendingBookings();
            LoadActiveBookings();
            LoadUsers();
        }

        private void LoadPendingBookings()
        {
            var bookings = new ObservableCollection<Booking>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT b.*, p.Name as PlaceName, u.Username, u.AvatarPath as UserAvatar 
                        FROM Bookings b
                        INNER JOIN Places p ON b.PlaceId = p.Id
                        INNER JOIN Users u ON b.UserId = u.Id
                        WHERE b.Status = @status
                        ORDER BY b.SelectedDate, b.StartTime";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", "ждем подтверждения администратора ;)");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var booking = new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PlaceId = reader.GetInt32(reader.GetOrdinal("PlaceId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                SelectedDate = reader.GetDateTime(reader.GetOrdinal("SelectedDate")),
                                StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                                EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                Place = new Place { Name = reader.GetString(reader.GetOrdinal("PlaceName")) },
                                User = new User
                                {
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    AvatarPath = reader.IsDBNull(reader.GetOrdinal("UserAvatar")) ?
                                        null : reader.GetString(reader.GetOrdinal("UserAvatar"))
                                }
                            };
                            bookings.Add(booking);
                        }
                    }
                }
            }
            PendingBookings = bookings;
        }
        private void LoadActiveBookings()
        {
            var bookings = new ObservableCollection<Booking>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT b.*, p.*, u.* 
                                FROM Bookings b
                                INNER JOIN Places p ON b.PlaceId = p.Id
                                INNER JOIN Users u ON b.UserId = u.Id
                                WHERE b.Status = @status
                                ORDER BY b.SelectedDate, b.StartTime";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", ApprovedStatus);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var booking = new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                // Заполнение остальных полей бронирования
                            };
                            bookings.Add(booking);
                        }
                    }
                }
            }
            ActiveBookings = bookings;
        }

        private void CompleteBooking(int bookingId)
        {
            var booking = ActiveBookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return;

            if (MessageBox.Show("Вы уверены, что хотите завершить бронирование?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = "UPDATE Bookings SET Status = @status WHERE Id = @id";
                            using (var command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@status", "Завершено");
                                command.Parameters.AddWithValue("@id", bookingId);
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            ActiveBookings.Remove(booking);
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        private void LoadUsers()
        {
            var users = new ObservableCollection<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Заменяем вызов хранимой процедуры на обычный SQL-запрос
                string query = @"SELECT 
                            Id, 
                            Username, 
                            Email, 
                            Phone, 
                            IsBlocked, 
                            AvatarPath 
                         FROM Users 
                         WHERE Id != @currentUserId 
                         ORDER BY Username";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@currentUserId", CurrentAdmin.Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                IsBlocked = reader.GetBoolean(reader.GetOrdinal("IsBlocked")),
                                AvatarPath = reader.IsDBNull(reader.GetOrdinal("AvatarPath")) ?
                                    null : reader.GetString(reader.GetOrdinal("AvatarPath"))
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            Users = users;
            if (SelectedUser != null)
            {
                SelectedUser = users.FirstOrDefault(u => u.Id == SelectedUser.Id);
            }
        }

        private void ApproveBooking(int bookingId)
        {
            var booking = PendingBookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return;

            // Проверка даты
            if (booking.SelectedDate < new DateTime(1753, 1, 1) || booking.SelectedDate > new DateTime(9999, 12, 31))
            {
                MessageBox.Show("Некорректная дата бронирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string updateBookingQuery = "UPDATE Bookings SET Status = @status WHERE Id = @id";
                        using (var command = new SqlCommand(updateBookingQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@status", ApprovedStatus);
                            command.Parameters.AddWithValue("@id", bookingId);
                            command.ExecuteNonQuery();
                        }

                        string updateConflictsQuery = @"UPDATE Bookings 
                                             SET Status = 'Отклонено'
                                             WHERE PlaceId = @placeId AND
                                                   SelectedDate = @date AND
                                                   StartTime < @endTime AND
                                                   EndTime > @startTime AND
                                                   Id != @id";
                        using (var command = new SqlCommand(updateConflictsQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@placeId", booking.PlaceId);
                            command.Parameters.AddWithValue("@date", booking.SelectedDate);
                            command.Parameters.AddWithValue("@startTime", booking.StartTime);
                            command.Parameters.AddWithValue("@endTime", booking.EndTime);
                            command.Parameters.AddWithValue("@id", bookingId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        PendingBookings.Remove(booking);
                        booking.Status = ApprovedStatus;
                        ActiveBookings.Add(booking);
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при обновлении бронирования: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void RejectBooking(int bookingId)
        {
            var booking = PendingBookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "UPDATE Bookings SET Status = 'Отклонено' WHERE Id = @id";
                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", bookingId);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        PendingBookings.Remove(booking);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void CancelBooking(int bookingId)
        {
            var booking = ActiveBookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return;

            if (MessageBox.Show("Вы уверены, что хотите отменить бронирование?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = "UPDATE Bookings SET Status = 'Отклонено' WHERE Id = @id";
                            using (var command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@id", bookingId);
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            ActiveBookings.Remove(booking);
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        private void StartUserEdit()
        {
            if (SelectedUser == null) return;
            IsUserEditMode = true;
        }

        private void SaveUserChanges()
        {
            if (SelectedUser == null) return;

            if (string.IsNullOrWhiteSpace(TempUsername) || TempUsername.Length < 2 || TempUsername.Length > 50)
            {
                MessageBox.Show("Имя пользователя должно содержать минимум 3 символа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addr = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (string.IsNullOrWhiteSpace(TempEmail) || !addr.IsMatch(TempEmail))
            {
                MessageBox.Show("Введите корректный email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(TempPhone))
            {
                MessageBox.Show("Введите корректный телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TempPhone = string.Empty;
                return;
            }

            var regex = new Regex(@"^\+375\((29|33|25|)\)\d{7}$");
            if (!regex.IsMatch(TempPhone))
            {
                MessageBox.Show("Формат: +375(29/33/25/)1234567", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"UPDATE Users 
                                       SET Username = @username, 
                                           Email = @email, 
                                           Phone = @phone 
                                       WHERE Id = @id";
                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@username", TempUsername);
                            command.Parameters.AddWithValue("@email", TempEmail);
                            command.Parameters.AddWithValue("@phone", TempPhone);
                            command.Parameters.AddWithValue("@id", SelectedUser.Id);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        SelectedUser.Username = TempUsername;
                        SelectedUser.Email = TempEmail;
                        SelectedUser.Phone = TempPhone;

                        OnPropertyChanged(nameof(SelectedUser));
                        IsUserEditMode = false;
                        var updatedUsers = Users.ToList();
                        Users = new ObservableCollection<User>(updatedUsers);

                        MessageBox.Show("Данные пользователя успешно обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CancelUserEdit()
        {
            if (SelectedUser != null)
            {
                TempUsername = SelectedUser.Username;
                TempEmail = SelectedUser.Email;
                TempPhone = SelectedUser.Phone;
            }
            IsUserEditMode = false;
        }

        private void ChangeUserAvatar()
        {
            if (SelectedUser == null) return;

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Выберите изображение для аватарки"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = "UPDATE Users SET AvatarPath = @avatarPath WHERE Id = @id";
                            using (var command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@avatarPath", openFileDialog.FileName);
                                command.Parameters.AddWithValue("@id", SelectedUser.Id);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            SelectedUser.AvatarPath = openFileDialog.FileName;
                            OnPropertyChanged(nameof(SelectedUser));

                            var updatedUsers = Users.ToList();
                            Users = new ObservableCollection<User>(updatedUsers);
                            MessageBox.Show("Аватарка пользователя успешно изменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Ошибка при изменении аватарки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void ToggleUserBlock()
        {
            if (SelectedUser == null) return;

            var action = SelectedUser.IsBlocked ? "разблокировать" : "заблокировать";
            if (MessageBox.Show($"Вы уверены, что хотите {action} пользователя {SelectedUser.Username}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = "UPDATE Users SET IsBlocked = @isBlocked WHERE Id = @id";
                            using (var command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@isBlocked", !SelectedUser.IsBlocked);
                                command.Parameters.AddWithValue("@id", SelectedUser.Id);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            SelectedUser.IsBlocked = !SelectedUser.IsBlocked;
                            OnPropertyChanged(nameof(SelectedUser));

                            var updatedUsers = Users.ToList();
                            Users = new ObservableCollection<User>(updatedUsers);
                            MessageBox.Show($"Пользователь {SelectedUser.Username} успешно {(SelectedUser.IsBlocked ? "заблокирован" : "разблокирован")}!",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Ошибка при изменении статуса блокировки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void Logout()
        {
            Application.Current.Properties["CurrentUser"] = null;
            _navigation.NavigateTo<MainWindow>();
        }
    }
}