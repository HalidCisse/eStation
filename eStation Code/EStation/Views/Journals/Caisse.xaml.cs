using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Journals {

    
    internal partial class CaisseTransactions
    {

        public event EventHandler Refreshed;

        private DateTime _fromDate;
        private DateTime _toDate;

        public CaisseTransactions () {
            InitializeComponent();
        }

        
        public async Task Refresh (DateTime fromDate, DateTime toDate, bool shouldInvoke = false) {
            await Dispatcher.BeginInvoke(new Action(async () =>
            {
                _fromDate = fromDate;
                _toDate = toDate;

                _TRANS_LIST.ItemsSource = await App.Store.Economat.Finance.GetTransactions(_fromDate, _toDate);

                if (shouldInvoke)
                    Refreshed?.Invoke(null, EventArgs.Empty);              
            }));                        
        }


        private async void _ADD_TRANSACTION_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddTransaction { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_fromDate, _toDate , true);
        }


        private async void Annuler_Click (object sender, RoutedEventArgs e) {

            if(_TRANS_LIST.SelectedValue==null)
                return;

            try {
                   App.Store.Economat.Finance.CancelTransaction((Guid) _TRANS_LIST.SelectedValue);              
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }

            await Refresh(_fromDate, _toDate, true);
            ModernDialog.ShowMessage("Annuler avec Succès !", "EStation", MessageBoxButton.OK);            
        }


    }
}
