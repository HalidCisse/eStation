using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;


namespace EStation.Views.OilViews
{
    
    internal partial class OilPrelev 
    {
        private Guid _currentOil;


        public OilPrelev()
        {
            InitializeComponent();
        }


        public void Refresh(Guid currentOil)
        {
            _currentOil = currentOil;
            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _PRELEVS.ItemsSource = App.EStation.Oils.GetPrelevCards(new List<Guid> {_currentOil}, DateTime.Today.AddDays(-7), DateTime.Today);
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
