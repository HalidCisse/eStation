using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CLib;
using eStation.Ext;
using eStationCore.Model.Oil.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.OilViews
{
   
    internal partial class OilDeliveries 
    {

        private List<Guid> _currentOils;


        public OilDeliveries()
        {
            InitializeComponent();
        }


        public async Task Refresh(List<Guid> currentOil)
        {
            _currentOils = currentOil;
            
            _STOCKS.ItemsSource = await App.Store.Oils.GetOilDeliveries(_currentOils, DateTime.Today.AddMonths(-3), DateTime.Today);
            _TITLE_TEXT.Text = "LIVRAISONS (";

            foreach (var oilGuid in currentOil)
                _TITLE_TEXT.Text += $" { (await App.Store.Oils.Get(oilGuid))?.Libel.ToUpper()}";
            _TITLE_TEXT.Text += ")";           
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_currentOils.Any())return;            

            var wind = new AddOilDelivery(_currentOils.First(), Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_currentOils);
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {           
            if (_STOCKS.SelectedItem == null) return;

            try
            {
                var card = ((OilDeliveryCard)_STOCKS.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer cette livraison de " + card.Supplier + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Oils.DeleteDelivery(card.OilDeliveryGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh(_currentOils);
            e.Handled = true;
        }
    }
}
