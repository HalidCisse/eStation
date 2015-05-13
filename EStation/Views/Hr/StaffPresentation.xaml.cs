using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EStation.Views.Hr
{
    /// <summary>
    /// Presentation du Staff
    /// </summary>
    public partial class StaffPresentation
    {

        /// <summary>
        /// Fire When date changed, (Guid)sender as Staff Guid
        /// </summary>
        public event EventHandler DateSelectionChanged;

        private Guid _staffGuid;

        /// <summary>
        /// Guid du Staff a Afficher
        /// </summary>
        public Guid StaffGuid
        {
            set
            {
                new Task(() => Dispatcher.BeginInvoke(new Action(() =>
                {
                    _staffGuid                = value;
                    _FROM_PICKER.SelectedDate = FromDate = DateTime.Today.AddMonths(-5);
                    _TO_PICKER.SelectedDate   = ToDate   = DateTime.Today.AddMonths(1);                    
                    Refresh(FromDate, ToDate);
                    DateSelectionChanged?.Invoke(_staffGuid, new EventArgs());
                }))).Start();
            }

            get { return _staffGuid; }
        }


        /// <summary>
        /// Date de debut selectionner
        /// </summary>
        public DateTime FromDate { get; private set; }


        /// <summary>
        /// Date de fin Selectionner
        /// </summary>
        public DateTime ToDate { get; private set; }


        /// <summary>
        /// Presentation du Staff
        /// </summary>
        public StaffPresentation()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Fetch Data
        /// </summary>
        public void Refresh(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate   = toDate;

            if (_staffGuid == Guid.Empty) return;

            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _TOTAL_PAID.Content = App.EStation.Economat.PayRoll.GetStaffTotalPaid(_staffGuid, FromDate, ToDate).ToString("0.##", CultureInfo.CurrentCulture)+ " dh";           
                _TOTAL_DUE.Content  = App.EStation.Economat.PayRoll.GetStaffTotalDue(_staffGuid);              
                _GRID.DataContext   = App.EStation.HumanResource.GetStaff(_staffGuid);
            }))).Start();
        }


        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_staffGuid == Guid.Empty) return;

            if ((FromDate == _FROM_PICKER.SelectedDate.GetValueOrDefault() && ToDate == _TO_PICKER.SelectedDate.GetValueOrDefault()) || _TO_PICKER.SelectedDate == null) return;
                       
            Refresh(_FROM_PICKER.SelectedDate.GetValueOrDefault(), _TO_PICKER.SelectedDate.GetValueOrDefault());
            DateSelectionChanged?.Invoke(_staffGuid, e);
        }


    }
}
