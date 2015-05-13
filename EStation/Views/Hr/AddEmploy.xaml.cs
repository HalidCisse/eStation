using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStationCore.Model.Hr.Entity;
using EStationCore.Model.Hr.Enums;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Hr
{
    /// <summary>
    /// Ajouter Un Employement Pour Un Staff
    /// </summary>
    public partial class AddEmploy 
    {
        private bool _isAdd;
        private int _errors;

        /// <summary>
        /// Ajouter Un Employement Pour Un Staff
        /// </summary>
        /// <param name="staffGuid"></param>
        /// <param name="employToModGuid"></param>
        public AddEmploy(Guid staffGuid, Guid employToModGuid)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (staffGuid == Guid.Empty)
                    {
                        ModernDialog.ShowMessage("Staff Not Found", "ERREUR", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    _CATEGORIE.ItemsSource     = App.EStation.HumanResource.AllCategories();
                    _GRADE.ItemsSource         = App.EStation.HumanResource.AllGrades();
                    _DEPARTEMENT.ItemsSource   = App.EStation.HumanResource.AllDepartements();
                    //_DIVISION.ItemsSource      = App.DataS.HumanResource.AllDivisions();
                    //_PROJET.ItemsSource        = App.DataS.HumanResource.AllProjets();
                    _REPORT_TO.ItemsSource     = App.EStation.HumanResource.AllStaffsNames();
                    _PAY_TYPE.ItemsSource      = EnumsHelper.GetAllValuesAndDescriptions<PayType>();
                    _SALARY_REC.ItemsSource    = EnumsHelper.GetAllValuesAndDescriptions<InstallmentRecurrence>();

                    if (employToModGuid == Guid.Empty)
                    {
                        //_MATIERE_NAME_VALIDATOR.ClasseGuid = classGuid;
                        _isAdd = true;

                        var data = new Employment
                        {
                            EmploymentGuid   = Guid.NewGuid(),
                            StaffGuid        = staffGuid,
                            Position         = "",
                            Category         = _CATEGORIE.Items.GetItemAt(0)   as string,
                            Grade            = _GRADE.Items.GetItemAt(0)       as string,
                            Departement      = _DEPARTEMENT.Items.GetItemAt(0) as string,
                            //Division         = _DIVISION.Items.GetItemAt(0)    as string,
                            //Project          = _PROJET.Items.GetItemAt(0)      as string,
                            ReportTo         = _REPORT_TO.Items.GetItemAt(0)   as string,
                            PayType          = PayType.SalaryOnly,
                            HourlyPay        = 0,
                            SalaryRecurrence = InstallmentRecurrence.Monthly,
                            StartDate        = DateTime.Today,
                            EndDate          = DateTime.Today.AddYears(1),
                            Description      = ""                            
                        };
                        _GRID.DataContext    = data;
                    }
                    else
                    {
                        var data = App.EStation.Economat.PayRoll.GetEmployment(employToModGuid);
                        //_MATIERE_NAME_VALIDATOR.ClasseGuid = data.ClasseGuid;
                        //_MATIERE_NAME_VALIDATOR.IsAdd = false;
                        _TITLE_TEXT.Text = "MODIFICATION";
                        _GRID.DataContext = data;

                        _SALARY_REC.IsEnabled = false;
                        _NUM_REC.IsEnabled    = false;
                        _PAY_TYPE.IsEnabled   = false;
                        _HOURLY_PAY.IsEnabled = false;
                        _DEBUT_EMP.IsEnabled  = false;
                    }
                }));
            }).Start();
        }
      

        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var emp = (Employment)_GRID.DataContext;

                if (_NUM_REC.Value != null) emp.EndDate = emp.StartDate.GetValueOrDefault().AddMonths((int)_NUM_REC.Value) ;

                if (_isAdd) App.EStation.Economat.PayRoll.AddEmployment(emp);
                else App.EStation.Economat.PayRoll.UpdateEmployment(emp);
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
            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "EStation" ,
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


        private void _PAY_TYPE_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_PAY_TYPE?.SelectedValue == null) return;
 
            //_HOURLY_PAY.IsEnabled = ((PayType) _PAY_TYPE.SelectedValue) == PayType.HoursWorked;
        }


    }
}
