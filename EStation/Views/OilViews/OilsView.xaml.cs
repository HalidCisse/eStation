using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using eLib;
using eStation.Ext;
using eStationCore.Model.Oil.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.OilViews
{
    
    internal partial class OilsView 
    {
      
        public event EventHandler HuileSelectionChanged;

        public OilsView()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }


        internal async Task Refresh()
        {
            _BUSY_INDICATOR.IsBusy = true;
            _HUILES.ItemsSource = await App.Store.Oils.GetOilsCards();
            _BUSY_INDICATOR.IsBusy = false;
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOil(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
           await Refresh();
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) 
            => HuileSelectionChanged?.Invoke(new List<Guid>(_HUILES.SelectedItems.Cast<OilCard>().Select(c => c.OilGuid)), EventArgs.Empty);

        private async void PriceBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var card = ((OilCard)((TextBox)sender).DataContext);
                await App.Store.Oils.ChangePrice(card.OilGuid, card.CurrentdoubleUnitPrice);
            }
            catch (Exception exception)
            {
                DebugHelper.WriteException(exception);
            }
            await Refresh();
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_HUILES.SelectedItem == null) return;

            try
            {
                var card = ((OilCard)_HUILES.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer " + card.Libel + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Oils.Delete(card.OilGuid))
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
