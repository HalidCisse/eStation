using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using EStationCore.Model.Oil.Views;
using Humanizer;

namespace EStation.Views.Journals
{
    
    public partial class OilPeriodCard 
    {
        public event EventHandler SelectionChanged;

        public DateTime FromDate => _FROM_PICKER.SelectedDate.GetValueOrDefault();

        public DateTime ToDate => _TO_PICKER.SelectedDate.GetValueOrDefault();

        public List<Guid> SelectedOils => new List<Guid>(_OILS.SelectedItems.Cast<OilCard>().Select(c => c.OilGuid));


        public OilPeriodCard()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                await Task.Run(() => Refresh(DateTime.Today.AddDays(-7), DateTime.Today));
                SelectionChanged?.Invoke(SelectedOils, EventArgs.Empty);
            }));
        }


        public async Task Refresh(DateTime fromDate, DateTime toDate) => await Dispatcher.BeginInvoke(new Action(() =>
        {
            _FROM_PICKER.SelectedDate = fromDate;
            _TO_PICKER.SelectedDate = toDate;

            _OILS.ItemsSource = App.Store.Oils.GetOilsCards();
            _OILS.SelectAll();
        }));


        private async void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(SelectedOils, EventArgs.Empty);
            await UpdateDashboard();
        }


        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(SelectedOils, EventArgs.Empty);
            await UpdateDashboard();
        }


        private async Task UpdateDashboard() => await Dispatcher.BeginInvoke(new Action(() =>
        {
            _TOTAL_GALLONS.Content = "Bidon".ToQuantity(App.Store.Oils.GetGallonsSold(SelectedOils, FromDate, ToDate));
            _TOTAL_SOLD.Content = App.Store.Oils.GetSold(SelectedOils, FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);

            _TOTAL_STOCK.Content = "Bidon".ToQuantity(App.Store.Oils.GetTotalDelivery(SelectedOils, FromDate, ToDate));
            _TOTAL_COST.Content = App.Store.Oils.GetTotalDeliveryCost(SelectedOils, FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
        }));
    }
}
