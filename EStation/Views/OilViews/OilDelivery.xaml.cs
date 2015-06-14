using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;



namespace EStation.Views.OilViews
{
   
    internal partial class OilDeliveries 
    {

        private List<Guid> _currentOils;


        public OilDeliveries()
        {
            InitializeComponent();
        }


        public void Refresh(List<Guid> currentOil)
        {
            _currentOils = currentOil;
            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _STOCKS.ItemsSource = App.Store.Oils.GetOilDeliveries(_currentOils, DateTime.Today.AddMonths(-3), DateTime.Today);
                _TITLE_TEXT.Text = "LIVRAISONS (";

                foreach (var oilGuid in currentOil)
                    _TITLE_TEXT.Text += $" {App.Store.Oils.Get(oilGuid)?.Libel.ToUpper()}";
                _TITLE_TEXT.Text += ")";
            }))).Start();
        }


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_currentOils.Any())return;            

            var wind = new AddOilDelivery(_currentOils.First(), Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_currentOils);
        }

    }
}
