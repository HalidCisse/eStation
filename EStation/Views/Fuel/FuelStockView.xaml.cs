using System;
using System.Threading.Tasks;
using System.Windows;


namespace EStation.Views.Fuel
{
   
    public partial class FuelStockView 
    {
        public FuelStockView()
        {
            InitializeComponent();
        }

        private void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e) => new Task(() =>
        { Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); })); }).Start();




    }
}
