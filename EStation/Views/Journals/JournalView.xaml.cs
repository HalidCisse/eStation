
using System;
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
                => await ChartOilSale.Refresh(_OIL_PERIOD.SelectedOils, _OIL_PERIOD.FromDate, _OIL_PERIOD.ToDate);

        private async void CaisseDetails_OnDateSelectionChanged(object sender, EventArgs e) 
                => await _TRANS_CARD.Refresh(FinanceCard.FromDate, FinanceCard.ToDate);

        private async void CaisseTransactions_OnRefreshed(object sender, EventArgs e) 
                =>await FinanceCard.Refresh();        

    }
}
