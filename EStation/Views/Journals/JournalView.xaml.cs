
using System;
using System.Threading.Tasks;
using System.Windows;

namespace EStation.Views.Journals
{
   
    internal partial class JournalView 
    {
        public JournalView()
        {
            InitializeComponent();
        }

        private async void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e) 
                => await Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }));

        private async void FuelPeriodCard_OnSelectionChanged(object sender, EventArgs e)            
                => await _FUELS_SALES.Refresh(_FUEL_PERIOD.SelectedFuels, _FUEL_PERIOD.FromDate,_FUEL_PERIOD.ToDate);

        private async void OilPeriodCard_OnSelectionChanged(object sender, EventArgs e)            
                => await _OIL_SALE.Refresh(_OIL_PERIOD.SelectedOils, _OIL_PERIOD.FromDate, _OIL_PERIOD.ToDate);

        private async void CaisseDetails_OnDateSelectionChanged(object sender, EventArgs e) 
                => await _TRANS_CARD.Refresh(_CAISSE_CARD.FromDate, _CAISSE_CARD.ToDate);

        private async void CaisseTransactions_OnRefreshed(object sender, EventArgs e) 
                =>await _CAISSE_CARD.Refresh();



        //public async void Refresh() => await Task.Run(() => _CAISSE_CARD.Refresh());
        //private void OilPeriodCard_OnSelectionChanged(object sender, EventArgs e) => new Task(() => Dispatcher.BeginInvoke(new Action(
        //      async () => await Task.Run(() => _OIL_SALE.Refresh(_OIL_PERIOD.SelectedOils, _OIL_PERIOD.FromDate, _OIL_PERIOD.ToDate))))).Start();





    }
}
