using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStation.Ext;
using EStationCore.Model.Hr.Enums;
using EStationCore.Model.Sale.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Journals
{

    internal partial class AddTransaction {

        private int _errors;

       
        public AddTransaction () {
            InitializeComponent();

            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => {

                    _TRANS_TYPE.ItemsSource=EnumsHelper.GetAllValuesAndDescriptions<TransactionType>();
                    _PAY_METHODE.ItemsSource=EnumsHelper.GetAllValuesAndDescriptions<PaymentMethode>();
                    _DATE_TRANS.DisplayDateStart = DateTime.Today.AddDays(-1);

                    var newData = new Transaction
                    {
                        Designation          = "",
                        PaidToward           = "",
                        Amount               = 0,
                        PaymentMethode       = PaymentMethode.Espece,
                        TransactionDate      = DateTime.Today,
                        TransactionReference = App.Store.Economat.Treasury.GetNewTransactionReference()
                    };
                    _GRID.DataContext=newData;
                }));
            }).Start();
        }

        
        private void save_Executed (object sender, ExecutedRoutedEventArgs e) {
            try {
                
                var dialog = new ModernDialog {
                    Title="EStation",
                    Content="Ete vous sure de confirmer cette transaction ?"
                };

                if(dialog.ShowDialogOkCancel()!=MessageBoxResult.OK)
                    return;

                var newTransaction = (Transaction) _GRID.DataContext;

                if ((TransactionType) (_TRANS_TYPE.SelectedValue) == TransactionType.Expense)
                    newTransaction.Amount = - newTransaction.Amount;
                
                App.Store.Economat.Treasury.NewTransaction(newTransaction);
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            ModernDialog.ShowMessage("Enregistrer avec Success !", "EStation", MessageBoxButton.OK);
            e.Handled=true;
            Close();
        }


        private void Save_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute=_errors==0;
            e.Handled=true;
        }


        private void Validation_Error (object sender, ValidationErrorEventArgs e) {
            if(e.Action==ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) => Close(); 


        private void _TRANS_TYPE_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _TITLE_TEXT.Text = (TransactionType)(_TRANS_TYPE.SelectedValue)==TransactionType.Expense ? "DEPENSE" : "RECETTES";


        private void _PAY_METHODE_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_PAY_METHODE.SelectedValue==null)
                return;

            switch(((PaymentMethode)_PAY_METHODE.SelectedValue)) {
                case PaymentMethode.Virement:
                    _VIREMENT_NAME.Text="N° VIREMENT";
                    break;
                case PaymentMethode.Cheque:
                    _VIREMENT_NAME.Text="N° CHEQUE";
                    break;
            }
        }
    }
}
