using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using eStation.Ext;
using eStationCore.Model.Customers.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Clients
{
    
    public partial class ClientsView 
    {
        public ClientsView()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async ()=> await Refresh()));           
        }

        private async Task Refresh()
        {
            try
            {
                var work = Task<IEnumerable<CustomerCard>>.Factory.StartNew(App.Store.Customers.GetCustomersCards);

                _BUSY_INDICATOR.IsBusy = true;

                await work;

                _BUSY_INDICATOR.IsBusy = false;

                _CLIENT_LIST.ItemsSource = work.Result;
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "Non Authoriser", MessageBoxButton.OK);
            }
            catch (Exception exx)
            {
                DebugHelper.WriteException(exx);
                ModernDialog.ShowMessage("ERREUR", "OoOPS", MessageBoxButton.OK);
            }
        }

        private async void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e)
            => await Dispatcher.BeginInvoke(new Action(()
                => NavigationService?.GoBack()));

        private async void ClientList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (_CLIENT_LIST?.SelectedItem == null) return;
            //await Dispatcher.BeginInvoke(new Action(() => { NavigationService?.Navigate(new CustomerDetails((CustomerCard)_CLIENT_LIST.SelectedItem)); }));           
        }

        private async void AddButon_Click(object sender, RoutedEventArgs e)
        {
            var wind = new NewClient(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private async void Details_Click(object sender, RoutedEventArgs e)
        {
            //if (_CLIENT_LIST?.SelectedItem == null)
            //    return;
            //await Dispatcher.BeginInvoke(new Action(() => { NavigationService?.Navigate(new CustomerDetails((CustomerCard)_CLIENT_LIST.SelectedItem)); }));
           
        }

        private async void Modifier_OnClick(object sender, RoutedEventArgs e)
        {
            if (_CLIENT_LIST?.SelectedItem == null)
                return;

            _BUSY_INDICATOR.IsBusy = false;
            var wind = new NewClient((Guid)_CLIENT_LIST.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private  void Archiver_OnClick(object sender, RoutedEventArgs e)
        {
            if (_CLIENT_LIST?.SelectedItem == null)
                return;

            var dialog = new ModernDialog
            {
                Title = "EStation",
                Content = "Ete vous sure de supprimer " + ((CustomerCard)_CLIENT_LIST.SelectedItem).FullName + " de la base de donneé"
            };

            if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                return;

            try
            {
                App.Store.Customers.Delete((Guid)_CLIENT_LIST.SelectedValue);
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage("Archiver avec Success !", "EStation",
                MessageBoxButton.OK);
        }

        private async void _SEARCH_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_SEARCH_BOX.Text))
                return;

            try
            {
                _BUSY_INDICATOR.IsBusy = true;
                await Dispatcher.BeginInvoke(new Action(() =>
                      {
                          _CLIENT_LIST.ItemsSource = App.Store.Customers.Search(_SEARCH_BOX.Text);
                          _BUSY_INDICATOR.IsBusy = false;
                      }));
               
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
            }
        }

        private async void _SEARCH_BOX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_SEARCH_BOX.Text))
                return;

            try
            {
                _BUSY_INDICATOR.IsBusy = true;
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    _CLIENT_LIST.ItemsSource = App.Store.Customers.Search(_SEARCH_BOX.Text);
                    _BUSY_INDICATOR.IsBusy = false;
                }));                
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
            }
        }
        
    }
}
