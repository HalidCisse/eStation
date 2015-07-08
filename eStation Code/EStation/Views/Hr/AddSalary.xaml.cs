using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using eStationCore.Model.Hr.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Hr
{
    /// <summary>
    /// Ajouter nouvelle renumeration
    /// </summary>
    public partial class AddSalary 
    {
        private bool _isAdd;
        private int _errors;

        /// <summary>
        /// Ajouter nouvelle renumeration
        /// </summary>
        /// <param name="employGuid"></param>
        /// <param name="salaryToModGuid"></param>
        public AddSalary(Guid employGuid, Guid salaryToModGuid)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (employGuid == Guid.Empty)
                    {
                        ModernDialog.ShowMessage("Employment Reference Invalid", "ERREUR", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    var employ = App.Store.Economat.PayRoll.GetEmployment(employGuid);

                    _START_SALARY.DisplayDateStart = DateTime.Today;
                    _END_SALARY.DisplayDateStart   = DateTime.Today;
                    _START_SALARY.DisplayDateEnd   = employ.EndDate;
                    _END_SALARY.DisplayDateEnd     = employ.EndDate;

                    if (salaryToModGuid == Guid.Empty)
                    {
                        _isAdd = true;

                        var data = new Salary
                        {
                            EmploymentGuid = employGuid,
                            Designation    = "",
                            Remuneration   = 0,
                            StartDate      = employ.StartDate,                   
                            EndDate        = employ.EndDate,                     
                            Description    =    ""                                                     
                        };
                        _GRID.DataContext = data;
                    }
                    else
                    {
                        var data = App.Store.Economat.PayRoll.GetSalary(salaryToModGuid);                        
                        _TITLE_TEXT.Text = "MODIFICATION";
                        _GRID.DataContext = data;

                        _START_SALARY.IsEnabled = false;
                        _END_SALARY.IsEnabled   = false;
                        _REMUNERATION.IsEnabled = false;
                    }
                }));
            }).Start();
        }
     
        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {             
                if (_isAdd) App.Store.Economat.PayRoll.AddSalary((Salary)_GRID.DataContext);
                else App.Store.Economat.PayRoll.CancelSalary((Salary)_GRID.DataContext);
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled = true;
                return;
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "EStation",
                MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }


        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }


        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }


        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
