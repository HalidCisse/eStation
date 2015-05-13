using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EStation.Views.Hr
{
    /// <summary>
    /// List des Employements
    /// </summary>
    internal partial class StaffEmployments
    {

        /// <summary>
        /// Fire When date changed, (Guid)sender as Staff Guid
        /// </summary>
        public event EventHandler EmploySelectionChanged;
        internal Guid SelectedEmployGuid
        {
            get
            {
                if (_EMPLOY_LIST.SelectedValue != null) return (Guid) _EMPLOY_LIST.SelectedValue;
                return Guid.Empty;
            }
        }

        private Guid _staffGuid;
        private DateTime _fromDate;
        private DateTime _toDate;
        private bool _isReadOnly;

        internal bool IsReadOnly {
            set {
                _ADD_EMPLOY.Visibility=value ? Visibility.Collapsed : Visibility.Visible;
                _isReadOnly = value;
            }
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Guid du Staff a Afficher
        /// </summary>
        internal Guid StaffGuid
        {
            set
            {
                new Task(() => Dispatcher.BeginInvoke(new Action(() =>
                {
                    _staffGuid = value;
                    _fromDate = DateTime.Today.AddYears(-1);
                    _toDate = DateTime.Today;
                    EmploySelectionChanged?.Invoke(value, new EventArgs());
                }))).Start();
            }
        }


        public StaffEmployments ()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Fetch Data
        /// </summary>
        internal void Refresh(Guid staffGuid, DateTime fromDate, DateTime toDate)
        {
            _staffGuid = staffGuid;
            _fromDate  = fromDate;
            _toDate    = toDate;

            if (_staffGuid == Guid.Empty)
            {
                _EMPLOY_LIST.ItemsSource = null;
                return;
            }

            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _EMPLOY_LIST.ItemsSource = App.EStation.Economat.PayRoll.GetEmployments(_staffGuid, _fromDate, _toDate);
                _EMPLOY_LIST.SelectedIndex = 0;
            }))).Start();
        }


        private void _ADD_EMPLOY_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddEmploy(_staffGuid, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_staffGuid, _fromDate, _toDate);
        }


        private void _EMPLOY_LIST_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            EmploySelectionChanged?.Invoke(SelectedEmployGuid, e);
        }


        private void _EMPLOY_LIST_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_isReadOnly) return;
         
            var wind = new AddEmploy(_staffGuid, SelectedEmployGuid) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_staffGuid, _fromDate, _toDate);
        }


    }
}
