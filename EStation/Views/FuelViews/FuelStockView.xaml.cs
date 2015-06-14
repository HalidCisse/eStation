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


        private void Citernes_OnCiterneSelectionChanged(object sender, EventArgs e)
        {
            if (sender == null) return;
          
            _CITERNE_STOCK.Refresh((Guid) sender);
        }

        private void OilsView_OnHuileSelectionChanged(object sender, EventArgs e)
        {
            if (sender == null) return;

            OilDeliveries.Refresh((List<Guid>)sender);
            _OIL_PRELEVS.Refresh((List<Guid>) sender);
        }


    }
}
