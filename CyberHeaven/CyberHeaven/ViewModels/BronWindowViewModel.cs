using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;
using Microsoft.EntityFrameworkCore;

namespace CyberHeaven.ViewModels
{
    public class BronWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AppDbContext _dbContext;
        private readonly LocalizationService _localization;
        public LocalizationService Localization { get; } = new LocalizationService();

        public ICommand SwitchToRussianCommand => new RelayCommand(_ =>
        {
            Localization.SetLanguage("ru");
            AppSettings.CurrentLanguage = "ru";
            OnPropertyChanged(nameof(PriceSortButtonContent));
        });

        public ICommand SwitchToEnglishCommand => new RelayCommand(_ =>
        {
            Localization.SetLanguage("en");
            AppSettings.CurrentLanguage = "en";
            OnPropertyChanged(nameof(PriceSortButtonContent));
        });

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
        private User _currentUser;

        // Данные бронирования
        private PlaceCategory _selectedCategory;
        private Place _selectedPlace;
        private string _startTime;
        private string _endTime;
        private string _promoCode;
        private decimal _totalPrice;
        private decimal _basePrice;
        private List<PlaceCategory> _placeCategories;
        private string _discountInfo;
        private int _appliedDiscountPercent;
        private bool _isPriceDescending;
        private string _userImagePath;
        private string _timeError;
        public string TimeError
        {
            get => _timeError;
            set => SetProperty(ref _timeError, value);
        }

        private bool _hasTimeError;
        public bool HasTimeError
        {
            get => _hasTimeError;
            set => SetProperty(ref _hasTimeError, value);
        }
        public BronWindowViewModel(INavigationService navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _dbContext = new AppDbContext();
            _localization = new LocalizationService();
            IsDarkTheme = AppSettings.IsDarkTheme;
            Localization.SetLanguage(AppSettings.CurrentLanguage);
            InitializeCommands();
            LoadUserData();
            InitializePlaceData();
            LoadUserImage();
            InitializeSampleData();

        }

        #region Commands
        public ICommand OpenBronCommand { get; private set; }
        public ICommand OpenSalesCommand { get; private set; }
        public ICommand OpenAuthCommand { get; private set; }
        public ICommand OpenMainCommand { get; private set; }
        public ICommand BookCommand { get; private set; }
        public ICommand ApplyPromoCommand { get; private set; }
        public ICommand TogglePlaceAvailabilityCommand { get; private set; }
        public ICommand AddPlaceCommand { get; private set; }
        public ICommand RemovePlaceCommand { get; private set; }
        public ICommand SavePlacesCommand { get; private set; }
        public ICommand SortByPriceDescCommand { get; private set; }
        public ICommand SortByCategoryAscCommand { get; private set; }
        public ICommand SelectPlaceCommand { get; private set; }
        public ICommand UnblockPlaceCommand { get; private set; }
        public ICommand OpenReviewCommand { get; private set; }

        #endregion

        #region Properties
        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }

        private Visibility _adminPanelVisibility;
        public Visibility AdminPanelVisibility
        {
            get => _adminPanelVisibility;
            set
            {
                if (_adminPanelVisibility != value)
                {
                    _adminPanelVisibility = value;
                    OnPropertyChanged(nameof(AdminPanelVisibility));
                  
                }
            }
        }
        public List<PlaceCategory> PlaceCategories
        {
            get => _placeCategories;
            set => SetProperty(ref _placeCategories, value);
        }

        public PlaceCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnPropertyChanged(nameof(AvailablePlaces));
                SelectedPlace = AvailablePlaces?.FirstOrDefault();
                CalculatePrice();
            }
        }

        public List<Place> AvailablePlaces => SelectedCategory?.Places?.ToList();
        public Place SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                SetProperty(ref _selectedPlace, value);
                CalculatePrice();
            }
        }

        public List<string> AvailableTimes { get; private set; }

        public string StartTime
        {
            get => _startTime;
            set
            {
                SetProperty(ref _startTime, value);
                CalculatePrice();
                CheckTimeAvailability();
            }
        }

        public string EndTime
        {
            get => _endTime;
            set
            {
                SetProperty(ref _endTime, value);
                CalculatePrice();
                CheckTimeAvailability();
            }
        }

        public string PromoCode
        {
            get => _promoCode;
            set
            {
                SetProperty(ref _promoCode, value);
                CalculatePrice();
                ApplyPromoCode();
            }
        }

        public decimal BasePrice
        {
            get => _basePrice;
            set => SetProperty(ref _basePrice, value);
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public string DiscountInfo
        {
            get => _discountInfo;
            set => SetProperty(ref _discountInfo, value);
        }
        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (value < DateTime.Today)
                {
                    MessageBox.Show("Нельзя выбрать дату раньше сегодняшнего дня", "Ошибка выбора даты",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SetProperty(ref _selectedDate, value))
                {
                    CalculatePrice();
                    LoadPlaces(); // Перезагружаем места при изменении даты
                }
            }
        }

        private DateTime _displayDateStart = DateTime.Today;
        public DateTime DisplayDateStart
        {
            get => _displayDateStart;
            private set => SetProperty(ref _displayDateStart, value);
        }
        public string PriceSortButtonContent => _isPriceDescending ? Localization["PoSale1"] : Localization["PoSale2"];
        #endregion

        #region Private Methods
        private void InitializeCommands()
        {
            OpenBronCommand = new RelayCommand(_ => _navigation.NavigateTo<BronWindow>());
            OpenSalesCommand = new RelayCommand(_ => _navigation.NavigateTo<SaleWindow>());
            OpenAuthCommand = new RelayCommand(_ => HandleAuthNavigation());
            BookCommand = new RelayCommand(BookPlace);
            ApplyPromoCommand = new RelayCommand(_ => ApplyPromoCode());
            TogglePlaceAvailabilityCommand = new RelayCommand(TogglePlaceAvailability);
            AddPlaceCommand = new RelayCommand(AddPlace);
            RemovePlaceCommand = new RelayCommand(RemovePlace);
            SavePlacesCommand = new RelayCommand(_ => SavePlaces());
            SortByPriceDescCommand = new RelayCommand(_ => SortByPrice());
            SortByCategoryAscCommand = new RelayCommand(_ => SortByCategory());
            SelectPlaceCommand = new RelayCommand(SelectPlace);
            UnblockPlaceCommand = new RelayCommand(UnblockPlace);
            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            OpenReviewCommand = new RelayCommand(_ => _navigation.NavigateTo<ReviewsWindow>());
        }
private void CheckTimeAvailability()
        {
            if (SelectedPlace == null || string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
            {
                ClearTimeError();
                return;
            }

            if (!TimeSpan.TryParse(StartTime, out var start) || !TimeSpan.TryParse(EndTime, out var end))
            {
                ClearTimeError();
                return;
            }

            if (end <= start)
            {
                TimeError = "Время окончания должно быть позже времени начала";
                HasTimeError = true;
                return;
            }

            // Получаем все бронирования для этого места и даты
            var bookings = _dbContext.Bookings
                .Where(b => b.PlaceId == SelectedPlace.Id &&
                           b.SelectedDate == SelectedDate &&
                           b.Status == "Подтверждено")
                .ToList();

            // Проверяем пересечение с существующими бронированиями
            foreach (var booking in bookings)
            {
                if (start < booking.EndTime && end > booking.StartTime)
                {
                    // Формируем более информативное сообщение об ошибке
                    SetTimeError(booking.StartTime, booking.EndTime);
                    return;
                }
            }

            ClearTimeError();
        }

        private void SetTimeError(TimeSpan bookedStart, TimeSpan bookedEnd)
        {
            TimeError = $"Выбранное время пересекается с существующим бронированием:\n" +
                       $"Занято с {bookedStart:hh\\:mm} до {bookedEnd:hh\\:mm}\n\n" +
                       $"Пожалуйста, выберите время:\n" +
                       $"• До {bookedStart:hh\\:mm}\n" +
                       $"• Или после {bookedEnd:hh\\:mm}";
            HasTimeError = true;
        }

        private void ClearTimeError()
        {
            TimeError = string.Empty;
            HasTimeError = false;
        }
        private void HandleAuthNavigation()
        {
            var currentUser = Application.Current.Properties["CurrentUser"] as User;

            if (currentUser == null)
                _navigation.NavigateTo<RegisterWindow>();
            else if (currentUser.Role == "user")
                _navigation.NavigateTo<UserCabinetWindow>();
            else if (currentUser.Role == "admin")
                _navigation.NavigateTo<AdminWindow>();
        }
        private void InitializeSampleData()
        {
            // Проверяем, есть ли уже категории в базе
            if (_dbContext.PlaceCategories.Any())
                return;

            var categories = new List<PlaceCategory>
    {
        new PlaceCategory
        {
            Name = "Обычные места",
            PricePerHour = 3.5M,
            Places = new List<Place>()

        },
        new PlaceCategory
        {
            Name = "Места с экраном-невидимкой",
            PricePerHour = 5,
            Places = new List<Place>()

        },
        new PlaceCategory
        {
            Name = "VIP места",
            PricePerHour = 7,
            Places = new List<Place>()
        },

          new PlaceCategory
        {
            Name = "игровая комната",
            PricePerHour = 55,
            Places = new List<Place>()
        }
    };

            try
            {
                // Включаем IDENTITY_INSERT только если действительно нужно задавать Id вручную
                _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT PlaceCategories ON");
                _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Places ON");
                _dbContext.PlaceCategories.AddRange(categories);
                _dbContext.SaveChanges();

                MessageBox.Show("Тестовые данные успешно загружены");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке тестовых данных: {ex.Message}");
                if (ex.InnerException != null)
                {
                    MessageBox.Show($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
            }
        }
        private void SelectPlace(object parameter)
        {
            if (parameter is Place place)
            {
                SelectedPlace = place;
            }
        }
        private void LoadUserData()
        {
            try
            {
                // Явная проверка и приведение типа
                if (Application.Current.Properties.Contains("CurrentUser") &&
                    Application.Current.Properties["CurrentUser"] is User user)
                {
                    // Обновляем данные из базы
                    _currentUser = _dbContext.Users .FirstOrDefault(u => u.Id == user.Id);

                    if (_currentUser != null)
                    {
                        bool isAdmin = _currentUser.Role?.Equals("admin", StringComparison.OrdinalIgnoreCase) == true;
                        AdminPanelVisibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;

                        Debug.WriteLine($"Текущий пользователь: {_currentUser.Username}, " +
                                      $"Роль: {_currentUser.Role}, " +
                                      $"Видимость: {AdminPanelVisibility}");
                    }
                }

                else
                {
                    Debug.WriteLine("Пользователь не найден в Application.Properties");
                    AdminPanelVisibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки пользователя: {ex.Message}");
                AdminPanelVisibility = Visibility.Collapsed;
            }
        }

        private void LoadUserImage()
        {
            if (Application.Current.Properties["CurrentUser"] is User currentUser)
            {
                if (currentUser.Role == "admin")
                {
                    UserImagePath = "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\admin_icon.png";
                }
                if (currentUser.Role == "user")
                {
                    UserImagePath = currentUser.AvatarPath;
                    AdminPanelVisibility = Visibility.Visible;
                }
                // Для обычного пользователя
                else
                {
                    UserImagePath = Application.Current.Properties["UserImagePath"] as string
                        ?? (_isDarkTheme
                            ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                            : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png");
                }

                AdminPanelVisibility = currentUser.Role == "admin"
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else
            {
                // Для неавторизованного пользователя
                UserImagePath = _isDarkTheme
                    ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                    : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png";
            }

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsDarkTheme))
                {
                    // Обновление иконки при смене темы
                    if (!(Application.Current.Properties["CurrentUser"] is User))
                    {
                        UserImagePath = _isDarkTheme
                            ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                            : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png";
                    }
                }
            };
        }

        private void InitializePlaceData()
        {
            LoadPlaces();
            AvailableTimes = Enumerable.Range(10, 12).Select(h => $"{h}:00").ToList();
        }

        private void LoadPlaces()
        {
            try
            {
                // Загружаем категории и места с текущим статусом блокировки
                PlaceCategories = _dbContext.PlaceCategories
                    .Include(pc => pc.Places)
                    .AsNoTracking()
                    .ToList();

                // Загружаем подтвержденные бронирования для выбранной даты
                var bookings = _dbContext.Bookings
                    .Where(b => b.SelectedDate == SelectedDate && b.Status == "Подтверждено")
                    .ToList();

                foreach (var category in PlaceCategories)
                {
                    foreach (var place in category.Places)
                    {
                        // Сохраняем исходное состояние блокировки (админская блокировка)
                        bool wasBlocked = place.IsBlocked;

                        // Сбрасываем только временные статусы
                        if (!wasBlocked)
                        {
                            place.Status = "Доступно";
                        }

                        // Получаем бронирования для этого места
                        var placeBookings = bookings.Where(b => b.PlaceId == place.Id).ToList();

                        if (placeBookings.Any())
                        {
                            // Формируем строку с занятыми интервалами
                            var busyTimes = string.Join(", ",
                                placeBookings.Select(b => $"{b.StartTime:hh\\:mm}-{b.EndTime:hh\\:mm}"));

                            // Обновляем статус, но сохраняем блокировку если она была
                            place.Status = wasBlocked
                                ? $"Заблокировано (Занято: {busyTimes})"
                                : $"Доступно (Занято: {busyTimes})";
                        }
                        else if (!wasBlocked)
                        {
                            // Если нет бронирований и нет блокировки - статус "Доступно"
                            place.Status = "Доступно";
                        }
                    }
                }

                SelectedCategory = PlaceCategories.FirstOrDefault();
                OnPropertyChanged(nameof(AvailablePlaces));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки мест: {ex.Message}");
            }
        }


        private void BookPlace(object _)
        {
            if (HasTimeError)
            {
                MessageBox.Show("Пожалуйста, выберите свободное время", "Ошибка");
                return;
            }
            if (_currentUser == null)
            {
                MessageBox.Show("Для бронирования необходимо войти в систему", "Ошибка");
                _navigation.NavigateTo<RegisterWindow>();
                return;
            }
            if (SelectedPlace.IsBlocked)
            {
                MessageBox.Show("Это место заблокировано администратором и недоступно для бронирования", "Ошибка");
                return;
            }
            if (!ValidateBookingData()) return;
            
            var booking = new Booking
            {
                UserId = _currentUser.Id,
                PlaceId = SelectedPlace.Id,
                BookingDate = DateTime.Now,
                SelectedDate = SelectedDate,
                StartTime = TimeSpan.Parse(StartTime),
                EndTime = TimeSpan.Parse(EndTime),
                TotalPrice = TotalPrice,
                Status = "ждем подтверждения администратора ;)",
                PromoCodeUsed = string.IsNullOrWhiteSpace(PromoCode) ? null : PromoCode
            };

            try
            {
                // Проверка на пересечение с существующими бронированиями
                if (IsTimeSlotBooked(SelectedPlace.Id, SelectedDate,
                    TimeSpan.Parse(StartTime), TimeSpan.Parse(EndTime)))
                {
                    MessageBox.Show("Это время уже занято другим бронированием", "Ошибка");
                    return;
                }

                _dbContext.Bookings.Add(booking);
                _dbContext.SaveChanges();
                string message = $"Бронирование создано! Дождитесь подтвреждение администратором\n" +
                           $"Место: {SelectedPlace.Name}\n" +
                           $"Время: {StartTime} - {EndTime}\n" +
                           $"Сумма: {TotalPrice} руб.\n" +
                           $"Статус: {booking.Status}";

                MessageBox.Show(message, "Бронирование");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при бронировании: {ex.Message}", "Ошибка");
            }
        }

        private bool ValidateBookingData()
        {
            if (SelectedPlace == null || string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return false;
            }
            if (SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Нельзя выбрать дату раньше сегодняшнего дня");
                return false;
            }
            if (!TimeSpan.TryParse(StartTime, out var start) || !TimeSpan.TryParse(EndTime, out var end))
            {
                MessageBox.Show("Некорректный формат времени!");
                return false;
            }
            if (IsTimeSlotBooked(SelectedPlace.Id, SelectedDate, start, end))
            {
                MessageBox.Show("Это время уже занято другим бронированием");
                return false;
            }
            if (end <= start)
            {
                MessageBox.Show("Время окончания должно быть позже времени начала!");
                return false;
            }

            return true;
        }

        private bool IsTimeSlotBooked(int placeId, DateTime date, TimeSpan start, TimeSpan end)
        {
            return _dbContext.Bookings.Any(b =>
                b.PlaceId == placeId &&
                b.SelectedDate == date &&
                b.Status == "Подтверждено" &&
                ((b.StartTime >= start && b.StartTime < end) ||  // Новое бронирование начинается во время существующего
                 (b.EndTime > start && b.EndTime <= end) ||     // Новое бронирование заканчивается во время существующего
                 (b.StartTime <= start && b.EndTime >= end)));  // Новое бронирование полностью внутри существующего
        }
        private void ApplyPromoCode()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PromoCode))
                {
                    DiscountInfo = "Введите промокод";
                    _appliedDiscountPercent = 0;
                    CalculatePrice();
                    return;
                }

                // Вариант 1: Сравнение без учета регистра на стороне сервера
                var promo = _dbContext.PromoCodes
                    .FirstOrDefault(p => p.Code.ToLower() == PromoCode.Trim().ToLower());

                // ИЛИ Вариант 2: Если нужно точное сравнение (быстрее, если в БД есть индекс)
                // var promo = _dbContext.PromoCodes
                //     .FirstOrDefault(p => p.Code == PromoCode.Trim());

                if (promo == null)
                {
                    DiscountInfo = "Промокод не найден";
                    _appliedDiscountPercent = 0;
                }
                else if (promo.DiscountPercent <= 0 || promo.DiscountPercent > 100)
                {
                    DiscountInfo = "Неверный размер скидки (должна быть от 1% до 100%)";
                    _appliedDiscountPercent = 0;
                }

                else
                {
                    DiscountInfo = $"Применена скидка {promo.DiscountPercent}% по промокоду '{promo.Title}'";
                    _appliedDiscountPercent = promo.DiscountPercent;
                }
            }
            catch (Exception ex)
            {
                DiscountInfo = "Ошибка проверки промокода";
                _appliedDiscountPercent = 0;
                Debug.WriteLine($"Error applying promo code: {ex.Message}");
            }
            finally
            {
                CalculatePrice();
            }
        }

        private void CalculatePrice()
        {
            if (SelectedPlace == null || string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
            {
                BasePrice = 0;
                TotalPrice = 0;
                return;
            }

            if (!TimeSpan.TryParse(StartTime, out var start) || !TimeSpan.TryParse(EndTime, out var end))
            {
                BasePrice = 0;
                TotalPrice = 0;
                return;
            }
            if (start.Hours < 10 || end.Hours > 21 || end <= start)
            {
                BasePrice = 0;
                TotalPrice = 0;
                return;
            }
            if (end <= start)
            {
                BasePrice = 0;
                TotalPrice = 0;
                return;
            }

            var hours = (decimal)(end - start).TotalHours;
            BasePrice = hours * SelectedCategory.PricePerHour;
            TotalPrice = BasePrice - (BasePrice * _appliedDiscountPercent / 100);
        }

        private void SortByPrice()
        {
            _isPriceDescending = !_isPriceDescending;
            PlaceCategories = _isPriceDescending
                ? PlaceCategories.OrderByDescending(c => c.PricePerHour).ToList()
                : PlaceCategories.OrderBy(c => c.PricePerHour).ToList();
            OnPropertyChanged(nameof(PriceSortButtonContent));
        }

        private void SortByCategory()
        {
            PlaceCategories = PlaceCategories
                .OrderBy(c => c.Name switch
                {
                    string s when s.Contains("Игровая комната") => 0,
                    string s when s.Contains("VIP") => 1,
                    string s when s.Contains("Места с экраном") => 2,
                    _ => 3
                })
                .ThenBy(c => c.Name)
                .ToList();
        }
        #endregion

        #region Admin Functions
        private void TogglePlaceAvailability(object parameter)
        {
            if (SelectedPlace == null) return;

            try
            {
                SelectedPlace.IsBlocked = !SelectedPlace.IsBlocked;
                SelectedPlace.Status = SelectedPlace.IsBlocked ? "Заблокировано" : "Доступно";

                var placeToUpdate = _dbContext.Places.Find(SelectedPlace.Id);
                if (placeToUpdate != null)
                {
                    placeToUpdate.IsBlocked = SelectedPlace.IsBlocked;
                    placeToUpdate.Status = SelectedPlace.Status;
                    _dbContext.SaveChanges();

                    // Force refresh
                    LoadPlaces();
                    OnPropertyChanged(nameof(AvailablePlaces));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void UnblockPlace(object parameter)
        {
            if (SelectedPlace == null || !SelectedPlace.IsBlocked) return;

            try
            {
                SelectedPlace.IsBlocked = false;
                SelectedPlace.Status = "Доступно";

                var placeToUpdate = _dbContext.Places.Find(SelectedPlace.Id);
                if (placeToUpdate != null)
                {
                    placeToUpdate.IsBlocked = false;
                    placeToUpdate.Status = "Доступно";
                    _dbContext.SaveChanges();

                    // Force refresh
                    LoadPlaces();
                    OnPropertyChanged(nameof(AvailablePlaces));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddPlace(object parameter)
        {
            if (SelectedCategory == null) return;

            try
            {
                // Get all places in the selected category
                var placesInCategory = _dbContext.Places
                    .Where(p => p.CategoryId == SelectedCategory.Id)
                    .ToList();

                // Determine the prefix based on category
                string prefix = SelectedCategory.Name switch
                {
                    string s when s.Contains("Обычные") => "A",
                    string s when s.Contains("экраном") => "B",
                    string s when s.Contains("VIP") => "V",
                    string s when s.Contains("Комната") => "K",
                    _ => "X"
                };

                // Find the highest number for this prefix
                int maxNumber = placesInCategory
                    .Where(p => p.Name.StartsWith(prefix))
                    .Select(p => int.TryParse(p.Name.Substring(prefix.Length), out int num) ? num : 0)
                    .DefaultIfEmpty(0)
                    .Max();

                var newPlace = new Place
                {
                    Name = $"{prefix}{maxNumber + 1}",
                    IsBlocked = false,
                    Status = "Доступно",
                    CategoryId = SelectedCategory.Id
                };

                _dbContext.Places.Add(newPlace);
                _dbContext.SaveChanges();

                // Refresh data
                LoadPlaces();
                SelectedPlace = newPlace;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void RemovePlace(object parameter)
        {
            if (SelectedPlace == null)
            {
                MessageBox.Show("Выберите место для удаления");
                return;
            }

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить место {SelectedPlace.Name}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                var placeToRemove = _dbContext.Places.Find(SelectedPlace.Id);
                if (placeToRemove != null)
                {
                    _dbContext.Places.Remove(placeToRemove);
                    _dbContext.SaveChanges();

                    // Обновляем данные
                    LoadPlaces();
                    SelectedPlace = AvailablePlaces?.FirstOrDefault();

                    MessageBox.Show("Место успешно удалено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void SavePlaces()
        {
            try
            {
                _dbContext.SaveChanges();
                MessageBox.Show("Изменения сохранены!", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка");
            }
        }
        #endregion
    }
}