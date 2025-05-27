using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CyberHeaven.Commands
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand OpenHelp = new RoutedUICommand(
            "Open Help",
            "OpenHelp",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1) // Назначим горячую клавишу F1
            });

        public static readonly RoutedUICommand RefreshData = new RoutedUICommand(
            "Refresh Data",
            "RefreshData",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control) // Ctrl+R
            });
    }
}