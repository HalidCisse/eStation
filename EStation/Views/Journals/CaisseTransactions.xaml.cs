using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Journals {

    
    internal partial class CaisseTransactions
    {

        public event EventHandler Refreshed;

        private DateTime _fromDate;
        private DateTime _toDate;

        public CaisseTransactions () {
            InitializeComponent();
        }


        
        public void Refresh ( DateTime fromDate, DateTime toDate) {

            _fromDate=fromDate;
            _toDate=toDate;
           
            new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                _TRANS_LIST.ItemsSource=App.Store.Economat.Treasury.GetTransactions(_fromDate, _toDate);
                Refreshed?.Invoke(null, EventArgs.Empty);
            }))).Start();
        }


        private void _ADD_TRANSACTION_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddTransaction { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_fromDate, _toDate);
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) {

            if(_TRANS_LIST.SelectedValue==null)
                return;

            try {
                   App.Store.Economat.Treasury.CancelTransaction((Guid) _TRANS_LIST.SelectedValue);              
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }

            Refresh(_fromDate, _toDate);
            ModernDialog.ShowMessage("Annuler avec Succès !", "EStation", MessageBoxButton.OK);            
        }


    }
}
