using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CyberHeaven.Controls
{
    public partial class TimeRangeSelector : UserControl
    {
        public static readonly DependencyProperty AvailableTimesProperty =
            DependencyProperty.Register(
                "AvailableTimes",
                typeof(IEnumerable<string>),
                typeof(TimeRangeSelector),
                new PropertyMetadata(new List<string>()));

        public static readonly DependencyProperty StartTimeProperty =
      DependencyProperty.Register(
          "StartTime",
          typeof(string),
          typeof(TimeRangeSelector),
          new FrameworkPropertyMetadata(
              defaultValue: null,
              FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
              propertyChangedCallback: null,
              coerceValueCallback: new CoerceValueCallback(CoerceStartTime)),
          new ValidateValueCallback(ValidateTime));

        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register(
                "EndTime",
                typeof(string),
                typeof(TimeRangeSelector),
                new FrameworkPropertyMetadata(
                    defaultValue: null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    propertyChangedCallback: null,
                    coerceValueCallback: new CoerceValueCallback(CoerceEndTime)),
                new ValidateValueCallback(ValidateTime));

        public static readonly DependencyProperty MinDurationProperty =
            DependencyProperty.Register(
                "MinDuration",
                typeof(TimeSpan),
                typeof(TimeRangeSelector),
                new PropertyMetadata(TimeSpan.FromHours(1)));

        // Остальные свойства и методы остаются без изменений
        public IEnumerable<string> AvailableTimes
        {
            get => (IEnumerable<string>)GetValue(AvailableTimesProperty);
            set => SetValue(AvailableTimesProperty, value);
        }

        public string StartTime
        {
            get => (string)GetValue(StartTimeProperty);
            set => SetValue(StartTimeProperty, value);
        }

        public string EndTime
        {
            get => (string)GetValue(EndTimeProperty);
            set => SetValue(EndTimeProperty, value);
        }

        public TimeSpan MinDuration
        {
            get => (TimeSpan)GetValue(MinDurationProperty);
            set => SetValue(MinDurationProperty, value);
        }

        public TimeRangeSelector()
        {
            InitializeComponent();
        }

        private static object CoerceStartTime(DependencyObject d, object baseValue)
        {
            var control = (TimeRangeSelector)d;
            var newStartTime = (string)baseValue;

            if (string.IsNullOrEmpty(newStartTime))
                return null;

            if (string.IsNullOrEmpty(control.EndTime))
                return newStartTime;

            if (TimeSpan.TryParseExact(newStartTime, @"hh\:mm", CultureInfo.InvariantCulture, out var start) &&
                TimeSpan.TryParseExact(control.EndTime, @"hh\:mm", CultureInfo.InvariantCulture, out var end))
            {
                if (start >= end)
                {
                    var correctedEnd = start.Add(control.MinDuration);
                    control.SetCurrentValue(EndTimeProperty, correctedEnd.ToString(@"hh\:mm"));
                }
            }

            return newStartTime;
        }

        private static bool ValidateTime(object value)
        {
            if (value == null)
                return true;

            if (!(value is string timeStr))
                return false;

            if (!TimeSpan.TryParseExact(timeStr, @"hh\:mm", CultureInfo.InvariantCulture, out _))
                return false;

            return true;
        }

        private static object CoerceEndTime(DependencyObject d, object baseValue)
        {
            var control = (TimeRangeSelector)d;
            var newEndTime = (string)baseValue;

            if (string.IsNullOrEmpty(newEndTime))
                return null;

            if (string.IsNullOrEmpty(control.StartTime))
                return newEndTime;

            if (TimeSpan.TryParseExact(newEndTime, @"hh\:mm", CultureInfo.InvariantCulture, out var end) &&
                TimeSpan.TryParseExact(control.StartTime, @"hh\:mm", CultureInfo.InvariantCulture, out var start))
            {
                if (end <= start)
                {
                    return start.Add(control.MinDuration).ToString(@"hh\:mm");
                }
            }

            return newEndTime;
        }
    }
}