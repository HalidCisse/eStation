using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using eStationCore.Model.Fuel.Views;
using Humanizer;

namespace eStation.Views.Journals
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
        }
   
        public async Task Refresh(DateTime fromDate, DateTime toDate) 
        {
            await Dispatcher.BeginInvoke(new Action(async () =>
             {
                 _FROM_PICKER.SelectedDateChanged -= DatePicker_OnSelectedDateChanged;
                 _TO_PICKER.SelectedDateChanged -= DatePicker_OnSelectedDateChanged;
                 _CITERNES.SelectionChanged -= Selector_OnSelectionChanged;

                 _FROM_PICKER.SelectedDate = fromDate;
                 _TO_PICKER.SelectedDate = toDate;

                 _CITERNES.ItemsSource = await App.Store.Citernes.GetFuelCards();
                 _CITERNES.SelectAll();

                 _FROM_PICKER.SelectedDateChanged += DatePicker_OnSelectedDateChanged;
                 _TO_PICKER.SelectedDateChanged += DatePicker_OnSelectedDateChanged;
                 _CITERNES.SelectionChanged += Selector_OnSelectionChanged;

                 await UpdateDashboard();
             }));          
        }

        private async void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e) 
            => await UpdateDashboard();

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) 
            => await UpdateDashboard();

        private async Task UpdateDashboard()
        {
            _TOTAL_LITER.Content = "Litre".ToQuantity((int) await App.Store.Fuels.GetLiterSoldAsync(SelectedFuels, FromDate, ToDate));//().ToString("0.##\\ L", CultureInfo.CurrentCulture);
            _TOTAL_SOLD.Content = (await App.Store.Fuels.GetSold(SelectedFuels, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            _TOTAL_STOCK.Content = "Litre".ToQuantity((int) await App.Store.Fuels.GetTotalDeliveryLiter(SelectedFuels, FromDate, ToDate)); //.ToString("0.##\\ L", CultureInfo.CurrentCulture);
            _TOTAL_COST.Content = (await App.Store.Fuels.GetTotalDeliveryCost(SelectedFuels, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            SelectionChanged?.Invoke(SelectedFuels, EventArgs.Empty);
        }
    }
}
