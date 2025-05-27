using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CyberHeaven.Models;
using CyberHeaven.Services;
using CyberHeaven.Views;

namespace CyberHeaven;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Загружаем курсоры с проверкой
        if (!LoadCursors())
        {
            // Fallback на системные курсоры
            Application.Current.Resources["DefaultCursor"] = Cursors.Arrow;
            Application.Current.Resources["PointerCursor"] = Cursors.Hand;
        }
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated(); // Создаст БД и таблицы, если их нет
        }
        base.OnStartup(e);
       
    }

    private bool LoadCursors()
    {
        try
        {
            string defaultCursorPath = @"D:\ООП\CyberHeaven\CyberHeaven\images\DefaultCursor.ani";
            string pointerCursorPath = @"D:\ООП\CyberHeaven\CyberHeaven\images\PointerCursor.ani";

            // Проверка существования файлов
            if (!File.Exists(defaultCursorPath) || !File.Exists(pointerCursorPath))
            {
                MessageBox.Show("Файлы курсоров не найдены!");
                return false;
            }

            // Загрузка с проверкой
            var defaultCursor = new Cursor(defaultCursorPath);
            var pointerCursor = new Cursor(pointerCursorPath);

            // Проверка, что курсоры загрузились
            if (defaultCursor == null || pointerCursor == null)
            {
                MessageBox.Show("Ошибка загрузки курсоров");
                return false;
            }

            Application.Current.Resources["DefaultCursor"] = defaultCursor;
            Application.Current.Resources["PointerCursor"] = pointerCursor;

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки курсоров: {ex.Message}");
            return false;
        }
    }
}




