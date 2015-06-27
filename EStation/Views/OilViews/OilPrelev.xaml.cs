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


        public async Task Refresh(List<Guid> oilsGuids)
        {
            _currentOils = oilsGuids ;
           
                _PRELEVS.ItemsSource = await App.Store.Oils.GetPrelevCards(_currentOils, DateTime.Today.AddDays(-7), DateTime.Today);
                _TITLE_TEXT.Text = "PRELEVEMENTS (" ;

                foreach (var oilGuid in oilsGuids)
                    _TITLE_TEXT.Text += $" {(await App.Store.Oils.Get(oilGuid))?.Libel.ToUpper()}";
                _TITLE_TEXT.Text += ")";
        }
         
  
        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOilPrelev { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_currentOils);
        }

    }
}
