using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using eStationCore.Model.Oil.Views;
using Humanizer;

namespace eStation.Views.Journals
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
        }


        public async Task Refresh(DateTime fromDate, DateTime toDate)
        {
           await Dispatcher.BeginInvoke(new Action(async () =>
           {
               _FROM_PICKER.SelectedDateChanged -= DatePicker_OnSelectedDateChanged;
               _TO_PICKER.SelectedDateChanged -= DatePicker_OnSelectedDateChanged;
               _OILS.SelectionChanged -= Selector_OnSelectionChanged;

               _FROM_PICKER.SelectedDate = fromDate;
               _TO_PICKER.SelectedDate = toDate;

                _OILS.ItemsSource = await App.Store.Oils.GetOilsCards();
                _OILS.SelectAll();

               _FROM_PICKER.SelectedDateChanged += DatePicker_OnSelectedDateChanged;
               _TO_PICKER.SelectedDateChanged += DatePicker_OnSelectedDateChanged;
               _OILS.SelectionChanged += Selector_OnSelectionChanged;

               await UpdateDashboard();
           }));                      
        }


        private async void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
            => await UpdateDashboard();


        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            => await UpdateDashboard();


        private async Task UpdateDashboard() 
        {
            _TOTAL_GALLONS.Content = "Bidon".ToQuantity(await App.Store.Oils.GetGallonsSold(SelectedOils, FromDate, ToDate));
            _TOTAL_SOLD.Content    = (await App.Store.Oils.GetSold(SelectedOils, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            _TOTAL_STOCK.Content   = "Bidon".ToQuantity(await App.Store.Oils.GetTotalDelivery(SelectedOils, FromDate, ToDate));
            _TOTAL_COST.Content    = (await App.Store.Oils.GetTotalDeliveryCost(SelectedOils, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            SelectionChanged?.Invoke(SelectedOils, EventArgs.Empty);
        }

       
    }
}
