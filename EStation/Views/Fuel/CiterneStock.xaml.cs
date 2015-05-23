using System;
using System.Threading.Tasks;
using System.Windows;



namespace EStation.Views.Fuel
{
    internal partial class CiterneStock 
    {
        private Guid _currentCiterne;

        public CiterneStock()
        {
            InitializeComponent();           
        }


        public void Refresh(Guid currentCiterne)
        {
            _currentCiterne = currentCiterne;
            new Task(() => Dispatcher.BeginInvoke(new Action(()
                => _STOCKS.ItemsSource = App.EStation.Citernes.GetCiterneStocks(currentCiterne)))).Start();
        }


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {          
            var wind = new AddFuelStock(_currentCiterne, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_currentCiterne);
        }



    }
}
