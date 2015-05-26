using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CLib;
using EStation.Views;
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



        private void _LOGOUT_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            //FormsAuthentication.SignOut();
            Thread.CurrentPrincipal = null;

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _MAIN_FRAME.Navigate(new Login());

                    var cm = FindResource("UserPopup") as Popup;
                    if (cm == null) return;
                    cm.IsOpen = false;
                }));
            }).Start();

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


        private void _ESPACES_LIST_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)(sender)).SelectedValue == null) return;

            switch (((ListBox)(sender)).SelectedValue.ToString().ToEnum<UserSpace>())
            {
                case UserSpace.AdminSpace:
                    new Task(() => Dispatcher.BeginInvoke(new Action(()
                        => _MAIN_FRAME.NavigationService?.Navigate(new HomePage(), UriKind.Relative)))).Start();
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








    }
}
