using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using EStation.Ext;

namespace EStation.Ressources
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerNonUserCode]
    public partial class MyWindowStyle
    {
        #region sizing event handlers

        void OnSizeSouth(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.South); }
        void OnSizeNorth(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.North); }
        void OnSizeEast(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.East); }
        void OnSizeWest(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.West); }
        void OnSizeNorthWest(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.NorthWest); }
        void OnSizeNorthEast(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.NorthEast); }
        void OnSizeSouthEast(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.SouthEast); }
        void OnSizeSouthWest(object sender, MouseButtonEventArgs e) { OnSize(sender, SizingAction.SouthWest); }

        void OnSize(object sender, SizingAction action)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;
            sender.ForWindowFromTemplate(w =>
            {
                if (w.WindowState == WindowState.Normal)
                    DragSize(w.GetWindowHandle(), action);
            });
        }

        void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                sender.ForWindowFromTemplate(w => w.Close());
            }
            else
            {
                sender.ForWindowFromTemplate(w =>
                    SendMessage(w.GetWindowHandle(), WmSyscommand, (IntPtr)ScKeymenu, (IntPtr)' '));
            }
        }

        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.Close());
        }

        void MinButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = WindowState.Minimized);
        }
       
        void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = (w.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized);
        }

        void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                MaxButtonClick(sender, e);
            else if (e.LeftButton == MouseButtonState.Pressed)
                sender.ForWindowFromTemplate(w => w.DragMove());
        }

        void TitleBarMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            sender.ForWindowFromTemplate(w =>
            {
                if (w.WindowState != WindowState.Maximized) return;
                w.BeginInit();
                const double adjustment = 40.0;
                var mouse1 = e.MouseDevice.GetPosition(w);
                var width1 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                w.WindowState = WindowState.Normal;
                var width2 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                w.Left = (mouse1.X - adjustment) * (1 - width2 / width1);
                w.Top = -7;
                w.EndInit();
                w.DragMove();
            });
        }

        #endregion

        #region P/Invoke

        const int WmSyscommand = 0x112;
        const int ScSize = 0xF000;
        const int ScKeymenu = 0xF100;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        static void DragSize(IntPtr handle, SizingAction sizingAction)
        {
            SendMessage(handle, WmSyscommand, (IntPtr)(ScSize + sizingAction), IntPtr.Zero);
            SendMessage(handle, 514, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum SizingAction
        {
            /// <summary>
            /// 
            /// </summary>
            North = 3,
            /// <summary>
            /// 
            /// </summary>
            South = 6,
            /// <summary>
            /// 
            /// </summary>
            East = 2,
            /// <summary>
            /// 
            /// </summary>
            West = 1,
            /// <summary>
            /// 
            /// </summary>
            NorthEast = 5,
            /// <summary>
            /// 
            /// </summary>
            NorthWest = 4,
            /// <summary>
            /// 
            /// </summary>
            SouthEast = 8,
            /// <summary>
            /// 
            /// </summary>
            SouthWest = 7
        }

        #endregion
    }
}