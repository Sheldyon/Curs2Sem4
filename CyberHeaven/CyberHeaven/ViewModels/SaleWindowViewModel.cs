using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace CyberHeaven.ViewModels
{
    public class SaleWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AppDbContext _dbContext;
        private string _userImagePath;
        private Visibility _adminPanelVisibility = Visibility.Collapsed;
        private Visibility _createPromoVisibility = Visibility.Collapsed;
        private Visibility _editPromoVisibility = Visibility.Collapsed;
        private Visibility _deletePromoVisibility = Visibility.Collapsed;
        private string _newPromoTitle;
        private string _newPromoDescription;
        private string _newPromoCode;
        private int _newPromoDiscountPercent;

        private ObservableCollection<PromoCode> _promoCodes = new ObservableCollection<PromoCode>();
        private PromoCode _selectedPromoToEdit;
        private PromoCode _selectedPromoToDelete;

        public ObservableCollection<PromoCode> PromoCodes
        {
            get => _promoCodes;
            set => SetProperty(ref _promoCodes, value);
        }

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

        public Visibility CreatePromoVisibility
        {
            get => _createPromoVisibility;
            set => SetProperty(ref _createPromoVisibility, value);
        }

        public Visibility EditPromoVisibility
        {
            get => _editPromoVisibility;
            set => SetProperty(ref _editPromoVisibility, value);
        }

        public Visibility DeletePromoVisibility
        {
            get => _deletePromoVisibility;
            set => SetProperty(ref _deletePromoVisibility, value);
        }

        public string NewPromoTitle
        {
            get => _newPromoTitle;
            set => SetProperty(ref _newPromoTitle, value);
        }

        public string NewPromoDescription
        {
            get => _newPromoDescription;
            set => SetProperty(ref _newPromoDescription, value);
        }

        public string NewPromoCode
        {
            get => _newPromoCode;
            set => SetProperty(ref _newPromoCode, value);
        }

        public int NewPromoDiscountPercent
        {
            get => _newPromoDiscountPercent;
            set
            {
                SetProperty(ref _newPromoDiscountPercent, value);
                ValidateDiscount();
            }
        }

        public PromoCode SelectedPromoToEdit
        {
            get => _selectedPromoToEdit;
            set => SetProperty(ref _selectedPromoToEdit, value);
        }

        public PromoCode SelectedPromoToDelete
        {
            get => _selectedPromoToDelete;
            set => SetProperty(ref _selectedPromoToDelete, value);
        }
        private string _discountError;
        public string DiscountError
        {
            get => _discountError;
            set => SetProperty(ref _discountError, value);
        }
        public LocalizationService Localization { get; } = new LocalizationService();

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

        public ICommand OpenMainCommand { get; }
        public ICommand OpenBronCommand { get; }
        public ICommand OpenAuthCommand { get; }
        public ICommand CopyPromoCodeCommand { get; }
        public ICommand ToggleCreatePromoCommand { get; }
        public ICommand CreatePromoCommand { get; }
        public ICommand ToggleEditPromoCommand { get; }
        public ICommand SaveEditPromoCommand { get; }
        public ICommand ToggleDeletePromoCommand { get; }
        public ICommand DeletePromoCommand { get; }
        public ICommand OpenReviewCommand { get; }
        public ICommand ToggleThemeCommand => new RelayCommand(_ =>
        {
            IsDarkTheme = !IsDarkTheme;
        });

        private readonly List<string> _defaultPromoImages = new List<string>
        {
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\promo1.jpg",
            "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\promo2.jpg"
        };

        private Random _random = new Random();
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

        public SaleWindowViewModel(INavigationService navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _dbContext = new AppDbContext();
            IsDarkTheme = AppSettings.IsDarkTheme;
            Localization.SetLanguage(AppSettings.CurrentLanguage);

            OpenMainCommand = new RelayCommand(_ => _navigation.NavigateTo<MainWindow>());
            OpenBronCommand = new RelayCommand(_ => _navigation.NavigateTo<BronWindow>());
            OpenAuthCommand = new RelayCommand(_ => HandleAuthNavigation());
            CopyPromoCodeCommand = new RelayCommand(CopyPromoCodeToClipboard);
            OpenReviewCommand = new RelayCommand(_ => _navigation.NavigateTo<ReviewsWindow>());

            // Команды для создания промокода
            ToggleCreatePromoCommand = new RelayCommand(_ =>
            {
                CreatePromoVisibility = CreatePromoVisibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                if (CreatePromoVisibility == Visibility.Visible)
                {
                    NewPromoTitle = string.Empty;
                    NewPromoDescription = string.Empty;
                    NewPromoCode = string.Empty;
                    NewPromoDiscountPercent = 0;
                    EditPromoVisibility = Visibility.Collapsed;
                    DeletePromoVisibility = Visibility.Collapsed;
                }
            });

            CreatePromoCommand = new RelayCommand(CreatePromo);

            // Команды для редактирования промокода
            ToggleEditPromoCommand = new RelayCommand(_ =>
            {
                EditPromoVisibility = EditPromoVisibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                if (EditPromoVisibility == Visibility.Visible && PromoCodes.Count > 0)
                {
                    var promoToEdit = PromoCodes.First();
                    SelectedPromoToEdit = new PromoCode
                    {
                        Id = promoToEdit.Id,
                        Title = promoToEdit.Title,
                        Description = promoToEdit.Description,
                        Code = promoToEdit.Code,
                        DiscountPercent = promoToEdit.DiscountPercent
                    };
                }
            });

            SaveEditPromoCommand = new RelayCommand(SaveEditPromo);

            // Команды для удаления промокода
            ToggleDeletePromoCommand = new RelayCommand(_ =>
            {
               
                DeletePromoVisibility = DeletePromoVisibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                if (DeletePromoVisibility == Visibility.Visible && PromoCodes.Count > 0)
                {
                    SelectedPromoToDelete = PromoCodes.First();
                }
            });

            DeletePromoCommand = new RelayCommand(DeleteSelectedPromo);

            LoadUserData();
            LoadPromoCodes();
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

        private string GetRandomPromoImage()
        {
            return _defaultPromoImages[_random.Next(_defaultPromoImages.Count)];
        }

        private void LoadUserData()
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
                UserImagePath = _isDarkTheme
                    ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                    : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png";
            }

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsDarkTheme))
                {
                    if (!(Application.Current.Properties["CurrentUser"] is User))
                    {
                        UserImagePath = _isDarkTheme
                            ? "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogIn.png"
                            : "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\UserWithoutLogInlight.png";
                    }
                }
            };
        }
        private void ValidateDiscount()
        {
            if (!int.TryParse(NewPromoDiscountPercent.ToString(), out int discount))
            {
                DiscountError = "Скидка должна быть числом";
                MessageBox.Show("Скидка должна быть числовым значением", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                NewPromoDiscountPercent = 0;
                return;
            }
            if (NewPromoDiscountPercent > 100)
            {
                DiscountError = "Скидка не может превышать 100%";
                MessageBox.Show("Скидка не может превышать 100%", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                NewPromoDiscountPercent = 100; // Автоматически устанавливаем максимальное значение
            }
            else if (NewPromoDiscountPercent < 0)
            {
                DiscountError = "Скидка не может быть отрицательной";
                MessageBox.Show("Скидка не может быть отрицательной", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                NewPromoDiscountPercent = 0; // Автоматически устанавливаем минимальное значение
            }
            else
            {
                DiscountError = null;
            }
        }

        private void CreatePromo(object parameter)
        {
            if (NewPromoDiscountPercent > 100 || NewPromoDiscountPercent <= 0)
            {
                MessageBox.Show("Скидка должна быть в диапазоне от 1% до 100%", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(NewPromoDiscountPercent.ToString(), out int discount))
            {
                MessageBox.Show("Скидка должна быть числовым значением", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(NewPromoTitle) ||
                string.IsNullOrWhiteSpace(NewPromoDescription) ||
                string.IsNullOrWhiteSpace(NewPromoCode) ||
                NewPromoDiscountPercent <= 0) // Проверяем правильное свойство
            {
                MessageBox.Show("Заполните все обязательные поля для создания промокода");
                return;
            }

            var newPromo = new PromoCode
            {
                Title = NewPromoTitle,
                Description = NewPromoDescription,
                Code = NewPromoCode,
                DiscountPercent = NewPromoDiscountPercent, // Используем правильное свойство
                ImagePath = GetRandomPromoImage()
            };

            // Сохранение в БД
            try
            {
                _dbContext.PromoCodes.Add(newPromo);
                _dbContext.SaveChanges();
                PromoCodes.Add(newPromo);

                // Сброс полей
                NewPromoTitle = string.Empty;
                NewPromoDescription = string.Empty;
                NewPromoCode = string.Empty;
                NewPromoDiscountPercent = 0;
                CreatePromoVisibility = Visibility.Collapsed;

                MessageBox.Show("Промокод успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании промокода: {ex.Message}");
            }
        }

        private void SaveEditPromo(object parameter)
        {
            if (SelectedPromoToEdit == null)
            {
                MessageBox.Show("Не выбран промокод для редактирования");
                return;
            }
            if (!int.TryParse(SelectedPromoToEdit.DiscountPercent.ToString(), out int discount))
            {
                MessageBox.Show("Скидка должна быть числовым значением", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedPromoToEdit.DiscountPercent > 100 || SelectedPromoToEdit.DiscountPercent <= 0)
            {
                MessageBox.Show("Скидка должна быть в диапазоне от 1% до 100%", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var promoToUpdate = _dbContext.PromoCodes.FirstOrDefault(p => p.Id == SelectedPromoToEdit.Id);
            if (promoToUpdate == null)
            {
                MessageBox.Show("Ошибка: промокод не найден");
                return;
            }

            // Обновляем свойства
            promoToUpdate.Title = SelectedPromoToEdit.Title;
            promoToUpdate.Description = SelectedPromoToEdit.Description;
            promoToUpdate.Code = SelectedPromoToEdit.Code;
            promoToUpdate.DiscountPercent = SelectedPromoToEdit.DiscountPercent;

            try
            {
                _dbContext.SaveChanges();

                // Обновляем коллекцию
                var index = PromoCodes.IndexOf(PromoCodes.First(p => p.Id == SelectedPromoToEdit.Id));
                if (index >= 0)
                {
                    PromoCodes[index] = new PromoCode
                    {
                        Id = promoToUpdate.Id,
                        Title = promoToUpdate.Title,
                        Description = promoToUpdate.Description,
                        Code = promoToUpdate.Code,
                        DiscountPercent = promoToUpdate.DiscountPercent,
                        ImagePath = promoToUpdate.ImagePath
                    };
                }

                EditPromoVisibility = Visibility.Collapsed;
                MessageBox.Show("Изменения успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }

        private void DeleteSelectedPromo(object parameter)
        {
            if (SelectedPromoToDelete == null)
            {
                MessageBox.Show("Пожалуйста, выберите промокод для удаления");
                return;
            }

            var promoToDelete = SelectedPromoToDelete;
            var result = MessageBox.Show($"Вы уверены, что хотите удалить промокод '{promoToDelete.Title}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var dbPromo = _dbContext.PromoCodes.FirstOrDefault(p => p.Id == promoToDelete.Id);
                    if (dbPromo != null)
                    {
                        _dbContext.PromoCodes.Remove(dbPromo);
                        _dbContext.SaveChanges();
                        PromoCodes.Remove(promoToDelete);
                        DeletePromoVisibility = Visibility.Collapsed;
                        MessageBox.Show("Промокод успешно удален!");

                        if (PromoCodes.Count > 0)
                        {
                            SelectedPromoToDelete = PromoCodes.First();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении промокода: {ex.Message}");
                }
            }
        }

        private void ResetAllForms()
        {
            CreatePromoVisibility = Visibility.Collapsed;
            EditPromoVisibility = Visibility.Collapsed;
            DeletePromoVisibility = Visibility.Collapsed;

            NewPromoTitle = string.Empty;
            NewPromoDescription = string.Empty;
            NewPromoCode = string.Empty;
            NewPromoDiscountPercent = 0;
        }

        private void LoadPromoCodes()
        {
            try
            {
                var promos = _dbContext.PromoCodes.ToList();
                PromoCodes.Clear();
                foreach (var promo in promos)
                {
                    // Если у промокода нет изображения, назначаем случайное
                    if (string.IsNullOrEmpty(promo.ImagePath))
                    {
                        promo.ImagePath = GetRandomPromoImage();
                        _dbContext.SaveChanges();
                    }
                    PromoCodes.Add(promo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки промокодов: {ex.Message}");
                PromoCodes.Clear();
            }
        }

        private void CopyPromoCodeToClipboard(object parameter)
        {
            if (parameter is string promoCode)
            {
                Clipboard.SetText(promoCode);
                MessageBox.Show($"Промокод ' скопирован в буфер обмена!",
                    "Код скопирован",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}