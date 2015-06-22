using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using EStationCore.Model.Fuel.Views;

namespace EStation.Views.Journals
{
   
    public partial class FuelPeriodCard 
    {
       
        public event EventHandler SelectionChanged;

        public DateTime FromDate => _FROM_PICKER.SelectedDate.GetValueOrDefault();
        
        public DateTime ToDate => _TO_PICKER.SelectedDate.GetValueOrDefault();

        public List<Guid> SelectedFuels => new List<Guid>(_CITERNES.SelectedItems.Cast<FuelCard>().Select(c=> c.FuelGuid));


        public FuelPeriodCard()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                await Refresh(DateTime.Today.AddDays(-7), DateTime.Today);
                SelectionChanged?.Invoke(null, EventArgs.Empty);
            }));
        }
   
        public async Task Refresh(DateTime fromDate, DateTime toDate) 
        {
            _FROM_PICKER.SelectedDate = fromDate;
            _TO_PICKER.SelectedDate = toDate;

            _CITERNES.ItemsSource = await App.Store.Citernes.GetFuelCards();
            _CITERNES.SelectAll();
        }

        private async void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {           
            SelectionChanged?.Invoke(SelectedFuels, EventArgs.Empty);
            await UpdateDashboard();
        }

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(SelectedFuels, EventArgs.Empty);
            await UpdateDashboard();
        }

        private async Task UpdateDashboard() 
        {
            _TOTAL_LITER.Content =(await App.Store.Fuels.GetLiterSoldAsync(SelectedFuels, FromDate, ToDate)).ToString("0.##\\ L", CultureInfo.CurrentCulture);
            _TOTAL_SOLD.Content = (await App.Store.Fuels.GetSold(SelectedFuels, FromDate, ToDate)).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);

            _TOTAL_STOCK.Content = (await App.Store.Fuels.GetTotalDeliveryLiter(SelectedFuels, FromDate, ToDate)).ToString("0.##\\ L", CultureInfo.CurrentCulture);
            _TOTAL_COST.Content = (await App.Store.Fuels.GetTotalDeliveryCost(SelectedFuels, FromDate, ToDate)).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
        }
    }
}
