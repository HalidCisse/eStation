using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using CLib;
using EStation.Views.Common;
using EStation.Views.Security;
using EStationCore.Model.Security.Enums;

namespace EStation
{
    

    public partial class MainWindow 
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var card = App.Store.Meta.About;
            //_APP_NAME.Content = card.ProductName + (card.IsBeta ? " - Beta" : "");
        }

        private async void _LOGOUT_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            Thread.CurrentPrincipal = null;
            await Dispatcher.BeginInvoke(new Action(() =>
                  {
                      _MAIN_FRAME.Navigate(new Login());

                      var cm = FindResource("UserPopup") as Popup;
                      if (cm == null) return;
                      cm.IsOpen = false;
                  }));            
        }


        private void _POPUP_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            var cm = FindResource("UserPopup") as Popup;
            if (cm == null) return;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }


        private void UserPopup_OnOpened(object sender, EventArgs e)
        {
            ((Popup)sender).DataContext = App.CurrentUser;

            _USER_NAME_LABEL.Content = App.CurrentUser.FullName;
        }

        private void AboutPopup_OnOpened(object sender, EventArgs e) 
            => ((Popup)sender).DataContext = App.Store.Meta.About;

        private void SettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            var cm = FindResource("AboutPopup") as Popup;
            if (cm == null) return;
            cm.PlacementTarget = this;
            cm.IsOpen = true;           
        }


        private async void _ESPACES_LIST_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)(sender)).SelectedValue == null) return;

            switch (((ListBox)(sender)).SelectedValue.ToString().ToEnum<UserSpace>())
            {
                case UserSpace.AdminSpace:
                    await Dispatcher.BeginInvoke(new Action(()
                         => _MAIN_FRAME.NavigationService?.Navigate(new HomePage(), UriKind.Relative)));
                    break;
                //case UserSpace.SecretaireSpace:
                //    new Task(() => Dispatcher.BeginInvoke(new Action(()
                //        => _MAIN_FRAME.NavigationService?.Navigate(new TeacherSpace(), UriKind.Relative)))).Start();
                //    break;
                //case UserSpace.EconomatSpace:
                //    new Task(() => Dispatcher.BeginInvoke(new Action(()
                //        => _MAIN_FRAME.NavigationService?.Navigate(new EconomatView(false), UriKind.Relative)))).Start();
                //    break;
                //default:
                //    new Task(() => Dispatcher.BeginInvoke(new Action(()
                //        => _MAIN_FRAME.NavigationService?.Navigate(new Login(), UriKind.Relative)))).Start();
                //    break;
            }

            var cm = FindResource("UserPopup") as Popup;
            if (cm == null) return;
            cm.IsOpen = false;
        }


        private void _CHANGE_PASS_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new ChangePass { Owner = GetWindow(this) };
            wind.ShowDialog();
        }

        private void Hyperlink_OnRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            if (hyperlink == null) return;
            if (!Regex.IsMatch(hyperlink.NavigateUri.ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) return;
            var address = string.Concat("mailto:", hyperlink.NavigateUri.ToString(), "?subject=eStation&body=Bonjour,");
            try { System.Diagnostics.Process.Start(address); }
            catch { MessageBox.Show("Addresse e-mail invalide.", "E-mail error"); }
        }

        
    }
}
