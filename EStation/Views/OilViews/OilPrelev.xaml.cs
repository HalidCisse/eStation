using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;


namespace EStation.Views.OilViews
{
    
    internal partial class OilPrelev 
    {
        private List<Guid> _currentOils;


        public OilPrelev()
        {
            InitializeComponent();
        }


        public void Refresh(List<Guid> oilsGuids)
        {
            _currentOils = oilsGuids ;
            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _PRELEVS.ItemsSource = App.Store.Oils.GetPrelevCards(_currentOils, DateTime.Today.AddDays(-7), DateTime.Today);
                _TITLE_TEXT.Text = "PRELEVEMENTS (" ;

                foreach (var oilGuid in oilsGuids)
                    _TITLE_TEXT.Text += $" {App.Store.Oils.Get(oilGuid)?.Libel.ToUpper()}";
                _TITLE_TEXT.Text += ")";
            }))).Start();
        }
         
  
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOilPrelev { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_currentOils);
        }

    }
}
