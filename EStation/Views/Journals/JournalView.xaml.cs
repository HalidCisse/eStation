

using System;
using System.Threading.Tasks;

namespace EStation.Views.Journals
{
   
    internal partial class JournalView 
    {
        public JournalView()
        {
            InitializeComponent();
        }



        private void BACK_BUTTON_OnClick(object sender, System.Windows.RoutedEventArgs e) => new Task(() 
            => Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }))).Start();


        private void FuelPeriodCard_OnSelectionChanged(object sender, EventArgs e) 
            => _PRELEVS.Refresh(_FUEL_PERIOD.SelectedFuels, _FUEL_PERIOD.FromDate, _FUEL_PERIOD.ToDate);


        private void CaisseDetails_OnDateSelectionChanged(object sender, EventArgs e) 
            => _TRANS_CARD.Refresh(_CAISSE_CARD.FromDate, _CAISSE_CARD.ToDate);


        private void CaisseTransactions_OnRefreshed(object sender, EventArgs e)
            => _CAISSE_CARD.Refresh();

        
    }
}
