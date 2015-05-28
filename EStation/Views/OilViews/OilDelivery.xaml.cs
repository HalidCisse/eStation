using System;
using System.Threading.Tasks;
using System.Windows;



namespace EStation.Views.OilViews
{
   
    internal partial class OilDeliveries 
    {

        private Guid _currentOil;


        public OilDeliveries()
        {
            InitializeComponent();
        }


        public void Refresh(Guid currentOil)
        {
            _currentOil = currentOil;
            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _STOCKS.ItemsSource = App.EStation.Oils.GetOilDeliveries(_currentOil);
                _TITLE_TEXT.Text = "LIVRAISONS " + App.EStation.Oils.Get(_currentOil)?.Libel.ToUpper();
            }))).Start();
        }


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOilDelivery(_currentOil, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_currentOil);
        }

    }
}
