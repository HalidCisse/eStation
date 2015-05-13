using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EStationCore.Model.Hr.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Hr
{

    /// <summary>
    ///Liste des salaires des staffs
    /// </summary>
    public partial class StaffFinanceView
    {
        public bool IsReadOnly {
            set {
                _PAYCHECKS_LIST.ContextMenu.Visibility=value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private Guid _staffGuid;
        private DateTime? _fromDate;
        private DateTime? _toDate;

        /// <summary>
        /// Liste des salaires des staffs
        /// </summary>
        public StaffFinanceView () {
            InitializeComponent();
        }


        /// <summary>
        /// Fetch Data
        /// </summary>
        public void Refresh (Guid staffGuid, DateTime? fromDate, DateTime? toDate)
        {
            _staffGuid = staffGuid;
            _fromDate  = fromDate;
            _toDate    = toDate;

            new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                _PAYCHECKS_LIST.ItemsSource = App.EStation.Economat.PayRoll.GetPayrolls(_staffGuid, _fromDate, _toDate);
            }))).Start();
        }


        private void _PAY_SALARY_OnClick (object sender, RoutedEventArgs e) {

        }


        private void PayContext_OnOpened (object sender, RoutedEventArgs e) {
            if (_PAYCHECKS_LIST.SelectedValue == null) return;

            if(!((PayrollCard)_PAYCHECKS_LIST.SelectedItem).IsPaid) {
                ((MenuItem)((ContextMenu)sender).Items.GetItemAt(0)).Visibility=Visibility.Visible;
                ((MenuItem)((ContextMenu)sender).Items.GetItemAt(1)).Visibility=Visibility.Collapsed;
            }
            else {
                ((MenuItem)((ContextMenu)sender).Items.GetItemAt(0)).Visibility=Visibility.Collapsed;
                ((MenuItem)((ContextMenu)sender).Items.GetItemAt(1)).Visibility=Visibility.Visible;
            }
        }


        private void Paycheck_Click (object sender, RoutedEventArgs e) {
           
           if (_PAYCHECKS_LIST.SelectedValue == null) return;

            var payroll = ((PayrollCard) _PAYCHECKS_LIST.SelectedItem);
        
            try {                
                if(!payroll.IsPaid)
                    App.EStation.Economat.PayRoll.Paycheck(payroll.PayrollGuid);
                else
                    App.EStation.Economat.PayRoll.CancelPaycheck(payroll.PayrollGuid);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            //ModernDialog.ShowMessage(!payroll.IsPaid ? "Enregistrer avec Succès !" : "Annuler avec Succès !", "ESchool",
            //   MessageBoxButton.OK);
            if (payroll.IsPaid)      
               ModernDialog.ShowMessage("Annuler avec Succès !", "EStation", MessageBoxButton.OK);

            Refresh(_staffGuid, _fromDate, _toDate);
        }


        private void _PAYCHECKS_LIST_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


    }
}
