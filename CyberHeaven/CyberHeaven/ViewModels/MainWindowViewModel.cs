using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;
using System.Collections.Generic;

namespace CyberHeaven.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private string _userImagePath;
        private readonly AppDbContext _dbContext;
        private Visibility _adminPanelVisibility = Visibility.Collapsed;
        private int _currentImageIndex = 0;
        private Storyboard _flipStoryboard;
        private bool _isFlipping;
        private string _currentApexImage;
        private string _currentMainImage;
        private bool _isDarkTheme = AppSettings.IsDarkTheme;

        // Изображения для темной темы
        private readonly List<string> _apexImagesDark = new List<string>
        {
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\dota.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\apex-legendsmain.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\cs.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\fortnite.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\inzoi.jpg",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\genshin.png"
        };

        private readonly List<string> _mainImagesDark = new List<string>
        {
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main1.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main2.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main3.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main4.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main5.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main6.png"
        };

        // Изображения для светлой темы
        private readonly List<string> _apexImagesLight = new List<string>
        {
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\dotalight.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\apex-legendsmainlight.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\cslight.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\fortnitelight.png",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\inzoilight.jpg",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\genshinlight.png"
        };

        private readonly List<string> _mainImagesLight = new List<string>
        {
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main1light.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main2light.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main3light.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main4light.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main5light.png",
            "D:\\ООП\\CursVDA\\CursVDA\\images\\main6light.png"
        };

        // Текущие списки изображений
        private List<string> _currentApexImages;
        private List<string> _currentMainImages;

        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }

        public Visibility AdminPanelVisibility
        {
            get => _adminPanelVisibility;
            set => SetProperty(ref _adminPanelVisibility, value);
        }

        public string CurrentApexImage
        {
            get => _currentApexImage;
            set => SetProperty(ref _currentApexImage, value);
        }

        public string CurrentMainImage
        {
            get => _currentMainImage;
            set => SetProperty(ref _currentMainImage, value);
        }

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
                        _currentApexImages = _apexImagesDark;
                        _currentMainImages = _mainImagesDark;
                    }
                    else
                    {
                        ThemeManager.ApplyLightTheme();
                        _currentApexImages = _apexImagesLight;
                        _currentMainImages = _mainImagesLight;
                    }
                    LoadCurrentImages();
                    UpdateUserImagePath();
                }
            }
        }

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

        public ICommand OpenBronCommand { get; }
        public ICommand OpenSalesCommand { get; }
        public ICommand OpenAuthCommand { get; }
        public ICommand PreviousImageCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand ToggleThemeCommand => new RelayCommand(_ => IsDarkTheme = !IsDarkTheme);
        public ICommand OpenReviewCommand { get; }

        // В конструкторе:
     

        public MainWindowViewModel(INavigationService navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _dbContext = new AppDbContext();

            // Инициализация текущих списков изображений
            _currentApexImages = _isDarkTheme ? _apexImagesDark : _apexImagesLight;
            _currentMainImages = _isDarkTheme ? _mainImagesDark : _mainImagesLight;

            Localization.SetLanguage(AppSettings.CurrentLanguage);
            if (AppSettings.IsDarkTheme)
                ThemeManager.ApplyDarkTheme();
            else
                ThemeManager.ApplyLightTheme();

            OpenBronCommand = new RelayCommand(_ => _navigation.NavigateTo<BronWindow>());
            OpenSalesCommand = new RelayCommand(_ => _navigation.NavigateTo<SaleWindow>());
            OpenAuthCommand = new RelayCommand(_ => HandleAuthNavigation());
            OpenReviewCommand = new RelayCommand(_ => _navigation.NavigateTo<ReviewsWindow>());
            InitializeFlipAnimation();
            ApplyTheme();
            PreviousImageCommand = new RelayCommand(PreviousImage);
            NextImageCommand = new RelayCommand(NextImage);
            UpdateUserAndAdminState();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsDarkTheme))
                {
                    UpdateUserImagePath();
                }
            };

            LoadCurrentImages();
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

        private void UpdateUserAndAdminState()
        {
            if (Application.Current.Properties["CurrentUser"] is User currentUser)
            {
                if (currentUser.Role == "admin")
                {
                    UserImagePath = "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\admin_icon.png";
                    AdminPanelVisibility = Visibility.Visible;
                }
                 if (currentUser.Role == "user")
                    {
                        UserImagePath = currentUser.AvatarPath;
                        AdminPanelVisibility = Visibility.Visible;
                    }
                    else
                {
                    UserImagePath = Application.Current.Properties["UserImagePath"] as string
                        ?? GetDefaultUserImage();
                    AdminPanelVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                UserImagePath = GetDefaultUserImage();
                AdminPanelVisibility = Visibility.Collapsed;
            }
        }

        private string GetDefaultUserImage()
        {
            return _isDarkTheme
                ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png";
        }

        private void UpdateUserImagePath()
        {
            if (!(Application.Current.Properties["CurrentUser"] is User))
            {
                UserImagePath = GetDefaultUserImage();
            }
        }

        public void ApplyTheme()
        {
            var mergedDictionaries = new ResourceDictionary();
            var themeUri = IsDarkTheme
                ? new Uri("/CyberHeaven;component/Resources/DarkTheme.xaml", UriKind.Relative)
                : new Uri("/CyberHeaven;component/Resources/LightTheme.xaml", UriKind.Relative);

            mergedDictionaries.Source = themeUri;

            var stringsUri = new Uri("/CyberHeaven;component/Resources/Strings.xaml", UriKind.Relative);
            var stringsDict = new ResourceDictionary { Source = stringsUri };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(mergedDictionaries);
            Application.Current.Resources.MergedDictionaries.Add(stringsDict);
        }

        private void LoadCurrentImages()
        {
            try
            {
                CurrentApexImage = _currentApexImages[_currentImageIndex];
                CurrentMainImage = _currentMainImages[_currentImageIndex];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображений: {ex.Message}");
            }
        }

        private void InitializeFlipAnimation()
        {
            _flipStoryboard = new Storyboard();
            _flipStoryboard.Completed += (s, e) => _isFlipping = false;
        }

        private void StartFlipAnimation(UIElement target)
        {
            var animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(1.5),
                From = 0,
                To = 1,
                AutoReverse = false
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));

            _flipStoryboard.Children.Clear();
            _flipStoryboard.Children.Add(animation);
            _flipStoryboard.Begin();
        }

        private void NextImage(object parameter)
        {
            if (_isFlipping || parameter == null) return;
            _isFlipping = true;

            if (parameter is UIElement element)
                StartFlipAnimation(element);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                _currentImageIndex = (_currentImageIndex + 1) % _currentApexImages.Count;
                LoadCurrentImages();
                _isFlipping = false;
            };
            timer.Start();
        }

        private void PreviousImage(object parameter)
        {
            if (_isFlipping || parameter == null) return;
            _isFlipping = true;

            if (parameter is UIElement element)
                StartFlipAnimation(element);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                _currentImageIndex = (_currentImageIndex - 1 + _currentApexImages.Count) % _currentApexImages.Count;
                LoadCurrentImages();
                _isFlipping = false;
            };
            timer.Start();
        }
    }
}