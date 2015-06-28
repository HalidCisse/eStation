using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eStation.Views.Hr
{

    /// <summary>
    /// List des renumerations d'un Employement
    /// </summary>
    internal partial class StaffSalary
    {

        /// <summary>
        /// Fire When refreshed, (Guid)sender as salary Guid
        /// </summary>
        public event EventHandler SourceChanged;

        /// <summary>
        /// Fire When date changed, (Guid)sender as salary Guid
        /// </summary>
        public event EventHandler SalarySelectionChanged;

        /// <summary>
        /// Guid du salaire actuellement selectionner
        /// </summary>
        public Guid SelectedSalaryGuid
        {
            get {
                    if (_SALARY_LIST?.SelectedValue != null) return (Guid) _SALARY_LIST.SelectedValue;
                    return Guid.Empty;
                }
        }

        private Guid _employGuid;
        private bool _isReadOnly;

        public bool IsReadOnly {
            set {
                _ADD_SALARY.Visibility=value ? Visibility.Collapsed : Visibility.Visible;
                _isReadOnly = value;
            }
            get { return _isReadOnly; }
        }

        /// <summary>
        /// List des renumerations d'un Employement
        /// </summary>
        public StaffSalary()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Fetch Data
        /// </summary>
        public void Refresh(Guid employGuid)
        {
            if (employGuid == Guid.Empty)
            {
                _SALARY_LIST.ItemsSource = null;
                return;
            }
            _employGuid = employGuid;

            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _SALARY_LIST.ItemsSource = App.Store.Economat.PayRoll.GetSalaries(employGuid);
                SourceChanged?.Invoke(_SALARY_LIST?.SelectedValue, new EventArgs());
            }))).Start();
        }


        private void _SALARY_LIST_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_SALARY_LIST?.SelectedValue == null) return;

            SalarySelectionChanged?.Invoke(_SALARY_LIST.SelectedValue, e);
        }


        private void _SALARY_LIST_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {          
            if (_isReadOnly || SelectedSalaryGuid == Guid.Empty) return; 
                       
            var wind = new AddSalary(_employGuid, SelectedSalaryGuid) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();            
            Refresh(_employGuid);
        }


        private void _ADD_SALARY_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddSalary(_employGuid, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_employGuid);
        }


    }
}
