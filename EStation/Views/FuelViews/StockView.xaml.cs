using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace EStation.Views.FuelViews
{
    internal partial class StockView 
    {
        public StockView()
        {
            InitializeComponent();
        }


        private void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e) => new Task(() =>
        { Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); })); }).Start();


        private async void Citernes_OnCiterneSelectionChanged(object sender, EventArgs e)
        {
            if (sender == null) return;
          
           await _CITERNE_STOCK.Refresh((Guid) sender);
        }

        private async void OilsView_OnHuileSelectionChanged(object sender, EventArgs e)
        {
            if (sender == null) return;

            await OilDeliveries.Refresh((List<Guid>)sender);
            await _OIL_PRELEVS.Refresh((List<Guid>) sender);
        }

        private async void CarburantView_OnSelectionChanged(object sender, EventArgs e)
        {
            if (sender == null) return;

           await _CARB_PRELEVS.Refresh((List<Guid>) sender, DateTime.Today.AddDays(-7), DateTime.Today);
        }


    }
}
