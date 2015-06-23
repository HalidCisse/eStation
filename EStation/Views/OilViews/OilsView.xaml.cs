using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CLib;
using EStationCore.Model.Oil.Views;


namespace EStation.Views.OilViews
{
    
    internal partial class OilsView 
    {
      
        public event EventHandler HuileSelectionChanged;

        public OilsView()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }


        internal async Task Refresh() => _HUILES.ItemsSource = await App.Store.Oils.GetOilsCards();


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOil(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
           await Refresh();
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) 
            => HuileSelectionChanged?.Invoke(new List<Guid>(_HUILES.SelectedItems.Cast<OilCard>().Select(c => c.OilGuid)), EventArgs.Empty);

        private async void PriceBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var card = ((OilCard)((TextBox)sender).DataContext);
                await App.Store.Oils.ChangePrice(card.OilGuid, Convert.ToDouble(((TextBox)sender).Text));
            }
            catch (Exception exception)
            {
                DebugHelper.WriteException(exception);
            }
            await Refresh();
        }


    }
}
