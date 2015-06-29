using System;
using System.Threading.Tasks;
using System.Windows;
using CLib;
using eStation.Ext;
using eStationCore.Model.Fuel.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.FuelViews
{
    internal partial class CiterneDelivery 
    {
        private Guid _currentCiterne;

        public CiterneDelivery()
        {
            InitializeComponent();           
        }


        public async Task Refresh(Guid currentCiterne)
        {
            _BUSY_INDICATOR.IsBusy = true;
            _currentCiterne = currentCiterne;
            
            _STOCKS.ItemsSource = await App.Store.Citernes.GetCiterneStocks(currentCiterne);
            _TITLE_TEXT.Text = "LIVRAISONS " +(await App.Store.Citernes.Get(currentCiterne))?.Libel.ToUpper();
            _BUSY_INDICATOR.IsBusy = false;
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {          
            var wind = new AddFuelDelivery(_currentCiterne, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_currentCiterne);
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            return;

            if ( _STOCKS.SelectedItem == null) return;

            try
            {
                var card = ((FuelDeliveryCard)_STOCKS.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer cette livraison de " + card.Supplier + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Fuels.DeleteDelivery(card.FuelDeliveryGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh(_currentCiterne);
            e.Handled = true;
        }
    }
}
