using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberHeaven.Controls
{
    public partial class PasswordInputControl : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordInputControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordPropertyChanged, CoercePassword));

        public static readonly DependencyProperty ShowStrengthIndicatorProperty =
            DependencyProperty.Register("ShowStrengthIndicator", typeof(bool), typeof(PasswordInputControl),
                new PropertyMetadata(true));

        public static readonly DependencyProperty ShowRequirementsHintProperty =
            DependencyProperty.Register("ShowRequirementsHint", typeof(bool), typeof(PasswordInputControl),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register("IsPasswordVisible", typeof(bool), typeof(PasswordInputControl),
                new PropertyMetadata(false));

        // Измените объявление события в PasswordInputControl.xaml.cs
        public event EventHandler<EventArgs> PasswordChanged;
        public static readonly RoutedEvent PasswordStrengthCheckedEvent =
       EventManager.RegisterRoutedEvent(
           "PasswordStrengthChecked",
           RoutingStrategy.Bubble,
           typeof(RoutedEventHandler),
           typeof(PasswordInputControl));

        public static readonly RoutedEvent PasswordVisibilityTogglingEvent =
            EventManager.RegisterRoutedEvent(
                "PasswordVisibilityToggling",
                RoutingStrategy.Tunnel,
                typeof(RoutedEventHandler),
                typeof(PasswordInputControl));

        public static readonly RoutedEvent PasswordVisibilityToggledEvent =
            EventManager.RegisterRoutedEvent(
                "PasswordVisibilityToggled",
                RoutingStrategy.Direct,
                typeof(RoutedEventHandler),
                typeof(PasswordInputControl));
        public event RoutedEventHandler PasswordStrengthChecked
        {
            add { AddHandler(PasswordStrengthCheckedEvent, value); }
            remove { RemoveHandler(PasswordStrengthCheckedEvent, value); }
        }

        public event RoutedEventHandler PasswordVisibilityToggling
        {
            add { AddHandler(PasswordVisibilityTogglingEvent, value); }
            remove { RemoveHandler(PasswordVisibilityTogglingEvent, value); }
        }

        public event RoutedEventHandler PasswordVisibilityToggled
        {
            add { AddHandler(PasswordVisibilityToggledEvent, value); }
            remove { RemoveHandler(PasswordVisibilityToggledEvent, value); }
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public bool ShowStrengthIndicator
        {
            get => (bool)GetValue(ShowStrengthIndicatorProperty);
            set => SetValue(ShowStrengthIndicatorProperty, value);
        }

        public bool ShowRequirementsHint
        {
            get => (bool)GetValue(ShowRequirementsHintProperty);
            set => SetValue(ShowRequirementsHintProperty, value);
        }

        public bool IsPasswordVisible
        {
            get => (bool)GetValue(IsPasswordVisibleProperty);
            set => SetValue(IsPasswordVisibleProperty, value);
        }

        public PasswordInputControl()
        {
            InitializeComponent();
            UpdatePasswordVisibility();
            UpdateControlsVisibility();
        }

        private static object CoercePassword(DependencyObject d, object baseValue)
        {
            var password = (string)baseValue;

            // Коррекция - обрезаем слишком длинные пароли
            if (password != null && password.Length > 50)
            {
                return password.Substring(0, 50);
            }

            return password;
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PasswordInputControl)d;
            control.UpdatePasswordFields();
            control.UpdatePasswordStrength();
        }

        private void UpdatePasswordFields()
        {
            if (PasswordBox.Password != Password)
            {
                PasswordBox.Password = Password;
            }

            if (VisiblePasswordBox.Text != Password)
            {
                VisiblePasswordBox.Text = Password;
            }
        }

        private void UpdatePasswordStrength()
        {
            if (!ShowStrengthIndicator) return;

            int strength = CalculatePasswordStrength(Password);
            PasswordStrengthBar.Value = strength;

            // Изменяем цвет в зависимости от сложности
            PasswordStrengthBar.Foreground = strength switch
            {
                0 => Brushes.Red,
                1 => Brushes.OrangeRed,
                2 => Brushes.Orange,
                3 => Brushes.YellowGreen,
                4 => Brushes.Green,
                _ => Brushes.Gray
            };
            RoutedEventArgs args = new RoutedEventArgs(PasswordStrengthCheckedEvent);
            RaiseEvent(args);
        }
        
        private int CalculatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password)) return 0;

            int strength = 0;

            // Минимальная длина
            if (password.Length >= 8) strength++;

            // Наличие цифр
            if (password.Any(char.IsDigit)) strength++;

            // Наличие букв в разных регистрах
            if (password.Any(char.IsUpper) && password.Any(char.IsLower)) strength++;

            // Наличие спецсимволов
            if (password.Any(c => !char.IsLetterOrDigit(c))) strength++;

            return strength;
        }

        private void UpdatePasswordVisibility()
        {
            if (IsPasswordVisible)
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                VisiblePasswordBox.Visibility = Visibility.Visible;
                VisibilityIcon.Source = (ImageSource)FindResource("EyeOpenIcon");
            }
            else
            {
                PasswordBox.Visibility = Visibility.Visible;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
                VisibilityIcon.Source = (ImageSource)FindResource("EyeClosedIcon");
            }
        }

        private void UpdateControlsVisibility()
        {
            PasswordStrengthBar.Visibility = ShowStrengthIndicator ? Visibility.Visible : Visibility.Collapsed;
            PasswordHintText.Visibility = ShowRequirementsHint ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
            PasswordChanged?.Invoke(this, EventArgs.Empty);
        }

        private void VisiblePasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password = VisiblePasswordBox.Text;
            PasswordChanged?.Invoke(this, EventArgs.Empty);
        }
        private void ToggleVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the tunneling event (before the action)
            RoutedEventArgs tunnelingArgs = new RoutedEventArgs(PasswordVisibilityTogglingEvent);
            RaiseEvent(tunnelingArgs);

            IsPasswordVisible = !IsPasswordVisible;
            UpdatePasswordVisibility();

            // Raise the direct event (after the action)
            RoutedEventArgs directArgs = new RoutedEventArgs(PasswordVisibilityToggledEvent);
            RaiseEvent(directArgs);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ShowStrengthIndicatorProperty || e.Property == ShowRequirementsHintProperty)
            {
                UpdateControlsVisibility();
            }
            else if (e.Property == IsPasswordVisibleProperty)
            {
                UpdatePasswordVisibility();
            }
        }

    }
}