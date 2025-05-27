using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CyberHeaven.ViewModels
{
    public class ReviewWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AppDbContext _dbContext;
        private User _currentUser;
        private string _newComment;
        private int _selectedRating;
        private ObservableCollection<Review> _reviews;
        private int _currentFilterIndex;
        private readonly LocalizationService _localization;
        private ObservableCollection<Review> _allReviews; 
        public LocalizationService Localization { get; } = new LocalizationService();
        public ICommand FilterByRatingCommand { get; }
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
        public ICommand ClearStarFilterCommand => new RelayCommand(_ =>
        {
            SelectedStarFilter = null;
            ApplyFilter(_currentFilterIndex);
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

        public ObservableCollection<Review> Reviews
        {
            get => _reviews;
            set => SetProperty(ref _reviews, value);
        }

        public int CurrentFilterIndex
        {
            get => _currentFilterIndex;
            set
            {
                if (SetProperty(ref _currentFilterIndex, value))
                {
                    ApplyFilter(value);
                }
            }
        }

        public string NewComment
        {
            get => _newComment;
            set
            {
                SetProperty(ref _newComment, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                SetProperty(ref _selectedRating, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public User CurrentUser => _currentUser;
        public bool CanAddReview => _currentUser?.Role == "user";

        // Свойства для звезд рейтинга
        private bool _star1Checked;
        public bool Star1Checked
        {
            get => _star1Checked;
            set => SetProperty(ref _star1Checked, value);
        }

        private bool _star2Checked;
        public bool Star2Checked
        {
            get => _star2Checked;
            set => SetProperty(ref _star2Checked, value);
        }

        private bool _star3Checked;
        public bool Star3Checked
        {
            get => _star3Checked;
            set => SetProperty(ref _star3Checked, value);
        }

        private bool _star4Checked;
        public bool Star4Checked
        {
            get => _star4Checked;
            set => SetProperty(ref _star4Checked, value);
        }

        private bool _star5Checked;
        public bool Star5Checked
        {
            get => _star5Checked;
            set => SetProperty(ref _star5Checked, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                ApplySearch();
            }
        }

        public bool IsAdmin => _currentUser?.Role == "admin";
        private string _adminResponse;
      

        public string AdminResponse
        {
            get => _adminResponse;
            set => SetProperty(ref _adminResponse, value);
        }

        private Review _selectedReview;
        public Review SelectedReview
        {
            get => _selectedReview;
            set
            {
                SetProperty(ref _selectedReview, value);
                // Обновляем доступность команд при изменении выбранного отзыва
                CommandManager.InvalidateRequerySuggested();
            }
        }
        private int? _selectedStarFilter;
        public int? SelectedStarFilter
        {
            get => _selectedStarFilter;
            set
            {
                SetProperty(ref _selectedStarFilter, value);
                ApplyFilter(_currentFilterIndex); // Применяем фильтр при изменении
            }
        }

        // Команда для удаления отзыва
        public ICommand DeleteReviewCommand { get; }

        // Команда для добавления ответа администратора
        public ICommand AddAdminResponseCommand { get; }
        public ICommand SubmitReviewCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand OpenBronCommand { get; }
        public ICommand OpenSalesCommand { get; }
        public ICommand OpenAuthCommand { get; }
        public ICommand OpenMainCommand { get; }

        public ICommand SetRatingCommand { get; }

        private string _userImagePath;
        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }

        public ReviewWindowViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            _dbContext = new AppDbContext();
            _currentUser = Application.Current.Properties["CurrentUser"] as User;
            _localization = new LocalizationService();
            _currentFilterIndex = 0;
            IsDarkTheme = AppSettings.IsDarkTheme;
            Localization.SetLanguage(AppSettings.CurrentLanguage);
            LoadUserImage();
            // Инициализация команд
            OpenBronCommand = new RelayCommand(_ => _navigation.NavigateTo<BronWindow>());
            OpenSalesCommand = new RelayCommand(_ => _navigation.NavigateTo<SaleWindow>());
            OpenAuthCommand = new RelayCommand(_ => HandleAuthNavigation());
            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            SetRatingCommand = new RelayCommand(SetRating);
            FilterByRatingCommand = new RelayCommand(FilterByRating);

            SubmitReviewCommand = new RelayCommand(_ => SubmitReview(),
                _ => CanAddReview && SelectedRating > 0 && !string.IsNullOrWhiteSpace(NewComment));
            BackCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            DeleteReviewCommand = new RelayCommand(
             param => DeleteReview(param as Review),
             param => IsAdmin && param is Review);

            AddAdminResponseCommand = new RelayCommand(
                param => AddAdminResponse(param as Review),
                param => IsAdmin && param is Review && !string.IsNullOrWhiteSpace(AdminResponse));
            LoadReviewsAsync().ConfigureAwait(false);
        }
        
        private async Task DeleteReviewAsync(Review review)
        {
            if (review == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить этот отзыв?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Reviews.Remove(review);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    await LoadReviewsAsync();

                    MessageBox.Show("Отзыв успешно удален", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    MessageBox.Show($"Ошибка при удалении отзыва: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteReview(Review review)
        {
            DeleteReviewAsync(review).ConfigureAwait(false);
        }


        // Обновленный метод добавления ответа
        private async Task AddAdminResponseAsync(Review review)
        {
            if (review == null || string.IsNullOrWhiteSpace(AdminResponse)) return;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    review.AdminResponse = AdminResponse;
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    await LoadReviewsAsync();

                    AdminResponse = string.Empty;
                    MessageBox.Show("Ответ администратора добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    MessageBox.Show($"Ошибка при добавлении ответа: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Обновленный метод AddAdminResponse для вызова асинхронной версии
        private void AddAdminResponse(Review review)
        {
            AddAdminResponseAsync(review).ConfigureAwait(false);
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
                    
                }
                // Для обычного пользователя
                else
                {
                    UserImagePath = Application.Current.Properties["UserImagePath"] as string
                        ?? (_isDarkTheme
                            ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                            : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png");
                }

            
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
      private void SetRating(object parameter)
{
    if (parameter is string ratingStr && int.TryParse(ratingStr, out int rating))
    {
        SelectedRating = rating;
        UpdateRatingStars();
    }
}

// Метод для фильтрации по рейтингу
private void FilterByRating(object parameter)
{
    if (parameter is string ratingStr && int.TryParse(ratingStr, out int rating))
    {
        if (SelectedStarFilter == rating)
        {
            // Если уже выбран этот фильтр - сбрасываем
            SelectedStarFilter = null;
        }
        else
        {
            // Иначе применяем фильтр
            SelectedStarFilter = rating;
        }
    }
}
        private void UpdateRatingStars()
        {
            Star1Checked = SelectedRating >= 1;
            Star2Checked = SelectedRating >= 2;
            Star3Checked = SelectedRating >= 3;
            Star4Checked = SelectedRating >= 4;
            Star5Checked = SelectedRating >= 5;

            // Явно уведомляем об изменении всех свойств звездочек
            OnPropertyChanged(nameof(Star1Checked));
            OnPropertyChanged(nameof(Star2Checked));
            OnPropertyChanged(nameof(Star3Checked));
            OnPropertyChanged(nameof(Star4Checked));
            OnPropertyChanged(nameof(Star5Checked));
        }
        private void UpdateRatingVisual()
        {
            OnPropertyChanged(nameof(Star1Checked));
            OnPropertyChanged(nameof(Star2Checked));
            OnPropertyChanged(nameof(Star3Checked));
            OnPropertyChanged(nameof(Star4Checked));
            OnPropertyChanged(nameof(Star5Checked));
        }

        private void HandleAuthNavigation()
        {
            if (_currentUser == null)
                _navigation.NavigateTo<RegisterWindow>();
            else if (_currentUser.Role == "user")
                _navigation.NavigateTo<UserCabinetWindow>();
            else if (_currentUser.Role == "admin")
                _navigation.NavigateTo<AdminWindow>();
        }

        private async Task LoadReviewsAsync()
        {
            try
            {
                var reviews = await _dbContext.Reviews
                    .Include(r => r.User)
                    .Where(r => r.IsApproved)
                    .ToListAsync();

                _allReviews = new ObservableCollection<Review>(reviews); // Сохраняем полный список
                ApplyFilter(_currentFilterIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки отзывов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter(int filterType)
        {
            if (_allReviews == null) return;

            var query = _allReviews.AsQueryable();

            // Фильтрация по количеству звезд (если выбрано)
            if (SelectedStarFilter.HasValue)
            {
                query = query.Where(r => r.Rating == SelectedStarFilter.Value);
            }

            // Сортировка
            var filtered = filterType switch
            {
                0 => query.OrderByDescending(r => r.CreatedAt),    // Сначала новые (по умолчанию)
                1 => query.OrderBy(r => r.CreatedAt),              // Сначала старые
                2 => query.OrderByDescending(r => r.Rating),       // Положительные сначала
                3 => query.OrderBy(r => r.Rating),                 // Отрицательные сначала
                _ => query.OrderByDescending(r => r.CreatedAt)     // Fallback: сначала новые
            };

            Reviews = new ObservableCollection<Review>(filtered.ToList());
        }
        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadReviewsAsync().ConfigureAwait(false);
                return;
            }

            var searchTextLower = SearchText.ToLower();
            var filtered = _dbContext.Reviews
                .Include(r => r.User)
                .Where(r => r.IsApproved &&
                    (r.Comment.ToLower().Contains(searchTextLower) ||
                    (r.User.Username.ToLower().Contains(searchTextLower)) ||
                    (r.AdminResponse.ToLower().Contains(searchTextLower)))
                ).ToList();

            Reviews = new ObservableCollection<Review>(filtered);
            ApplyFilter(_currentFilterIndex);
        }
        private void ResetStars()
        {
            SelectedRating = 0;
            UpdateRatingVisual();
        }

        private async Task SubmitReviewAsync()
        {
            if (!CanAddReview) return;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var review = new Review
                    {
                        UserId = _currentUser.Id,
                        Rating = SelectedRating,
                        Comment = NewComment,
                        CreatedAt = DateTime.Now,
                        IsApproved = true,
                        AdminResponse = string.Empty
                    };

                    await _dbContext.Reviews.AddAsync(review);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Обновляем список отзывов
                    await LoadReviewsAsync();
                    NewComment = string.Empty;
                    ResetStars();

                    MessageBox.Show("Ваш отзыв успешно опубликован!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    MessageBox.Show($"Ошибка при добавлении отзыва: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SubmitReview()
        {
            SubmitReviewAsync().ConfigureAwait(false);
        }
    }
}