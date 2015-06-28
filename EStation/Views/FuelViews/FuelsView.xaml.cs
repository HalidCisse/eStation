using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStation.Ext;
using EStation.Views.Fuel;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;
using EStationCore.Model.Sale.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.FuelViews
{
    internal partial class FuelsView 
    {

        public event EventHandler SelectionChanged;


        private List<Guid> SelectedFuels => new List<Guid>(_CARBURANTS.SelectedItems.Cast<FuelCard>().Select(c => c.FuelGuid));


        public FuelsView()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }

       
        internal async Task Refresh()
        {         
            _CARBURANTS.ItemsSource = await App.Store.Fuels.GetFuelCards();
            _CARBURANTS.SelectAll();          
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddFuel(Guid.Empty){ Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private void CARBURANTS_OnSelectionChanged(object sender, SelectionChangedEventArgs e) 
            => SelectionChanged?.Invoke(SelectedFuels, EventArgs.Empty);

        private async void CARBURANTS_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_CARBURANTS.SelectedValue == null) return;
           
            var wind = new AddFuel((Guid) _CARBURANTS.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private async void ChangeLitrage(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var key =((FuelCard) (((DockPanel)menu.PlacementTarget).DataContext)).FuelGuid;

            var wind = new AddPrice(key) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private async void PriceBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var card = ((FuelCard)((TextBox) sender).DataContext);

                await  App.Store.Pompes.Post(new Price
                {
                    ActualPrice = card.CurrentDoublePrice, // Convert.ToDouble(((TextBox) sender).Text),
                    ProductGuid = card.FuelGuid,
                    FromDate = DateTime.Now
                });
            }
            catch (Exception exception)
            {
                DebugHelper.WriteException(exception);
            }
            await Refresh();
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_CARBURANTS.SelectedItem == null) return;

            try
            {
                var card = ((FuelCard)_CARBURANTS.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer " + card.Libel + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Fuels.Delete(card.FuelGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh();
            e.Handled = true;
        }
    }
}
