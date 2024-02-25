using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Messenger.Data
{




    public static class PopupBox
    {









        public static void Show(string message)
        {
            // Annahme: Diese Methode wird aus dem Haupt-UI-Thread aufgerufen
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.MainWindow;

                var popup = new Popup();
                var textBlock = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.LightGray,
                    Padding = new Thickness(10),
                    MaxWidth = 300,
                    TextWrapping = TextWrapping.Wrap
                };

                var border = new Border
                {
                    Child = textBlock,
                    BorderBrush = Brushes.Black,
                    Background = Brushes.BlueViolet,
                    BorderThickness = new Thickness(5),                  
                    CornerRadius = new CornerRadius(5),                   
                    Padding = new Thickness(10)
                };

                popup.Child = border;

                // Zentrieren Sie das Popup in der Mitte des MainWindow
                popup.PlacementTarget = mainWindow;
                popup.Placement = PlacementMode.Center;



               



                popup.IsOpen = true;

                // Schließen Sie das Popup nach einer bestimmten Zeit
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Tick += (sender, args) =>
                {
                    popup.IsOpen = false;
                    timer.Stop();
                };
                timer.Interval = TimeSpan.FromSeconds(5);
                timer.Start();
            });
        }











    }











}
