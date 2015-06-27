using System;
using System.Threading.Tasks;
using System.Windows;

namespace EStation.Views.FuelViews
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
            _currentCiterne = currentCiterne;
            
                _STOCKS.ItemsSource = await App.Store.Citernes.GetCiterneStocks(currentCiterne);
                _TITLE_TEXT.Text = "LIVRAISONS " +(await App.Store.Citernes.Get(currentCiterne))?.Libel.ToUpper();
                   
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {          
            var wind = new AddFuelDelivery(_currentCiterne, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_currentCiterne);
        }



    }
}
