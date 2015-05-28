using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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

            new Task(() => Dispatcher.BeginInvoke(new Action(async () =>
            {
                await Task.Run(() => Refresh(DateTime.Today.AddDays(-7), DateTime.Today));
                SelectionChanged?.Invoke(null, EventArgs.Empty);
            }))).Start();
        }

    
        public void Refresh(DateTime fromDate, DateTime toDate)
        {            
            new Task(() => 
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _FROM_PICKER.SelectedDate = fromDate;
                _TO_PICKER.SelectedDate = toDate;

                _CITERNES.ItemsSource = App.EStation.Citernes.GetFuelCards();
                _CITERNES.SelectAll();
            }))
            ).Start();
        }


        private async void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {           
            SelectionChanged?.Invoke(null, EventArgs.Empty);
            await Task.Run(() => UpdateDashboard());
        }


        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(null, EventArgs.Empty);
            await Task.Run(() => UpdateDashboard());
        }


        private void UpdateDashboard()
        {
            Thread.Sleep(1000);

            Dispatcher.BeginInvoke(new Action(() =>{
                _TOTAL_LITER.Content =App.EStation.Fuels.GetLiterSold(SelectedFuels, FromDate, ToDate).ToString("0.##\\ L", CultureInfo.CurrentCulture);
                _TOTAL_SOLD.Content = App.EStation.Fuels.GetSold(SelectedFuels, FromDate, ToDate) .ToString("0.##\\ dhs", CultureInfo.CurrentCulture);

                _TOTAL_STOCK.Content =App.EStation.Fuels.GetTotalDeliveryLiter(SelectedFuels, FromDate, ToDate).ToString("0.##\\ L", CultureInfo.CurrentCulture);
                _TOTAL_COST.Content =App.EStation.Fuels.GetTotalDeliveryCost(SelectedFuels, FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
            }));
        }

      
    }
}
