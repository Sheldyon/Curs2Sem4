using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace CyberHeaven.Views
{
    public class LogAction : TriggerAction<Button> 
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message",
                typeof(string),
                typeof(LogAction),
                new PropertyMetadata(""));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            try
            {
                string logPath = "D:\\ООП\\CyberHeaven\\CyberHeaven\\log.txt";
                string logMessage = $"{DateTime.Now}: {Message} (кнопка: {AssociatedObject.Content})";

                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                File.AppendAllText(logPath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка записи в лог: {ex.Message}");
            }
        }
    }
}