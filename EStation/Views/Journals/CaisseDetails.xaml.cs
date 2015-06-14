using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EStation.Views.Journals {
    /// <summary>
    /// Interaction logic for CaisseDetails.xaml
    /// </summary>
    public partial class CaisseDetails {

        /// <summary>
        /// Fire When date changed, (Guid)sender as Staff Guid
        /// </summary>
        public event EventHandler DateSelectionChanged;

        /// <summary>
        /// Date de debut selectionner
        /// </summary>
        public DateTime FromDate { get; private set; }


        /// <summary>
        /// Date de fin Selectionner
        /// </summary>
        public DateTime ToDate { get; private set; }


        /// <summary>
        /// Caisse Details
        /// </summary>
        public CaisseDetails () {
            InitializeComponent();

            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _FROM_PICKER.SelectedDate=FromDate=DateTime.Today.AddMonths(-1);
                _TO_PICKER.SelectedDate=ToDate=DateTime.Today.AddMonths(1);
                Refresh(FromDate, ToDate);
                DateSelectionChanged?.Invoke(null, new EventArgs());
            }))).Start();
        }

       
        public void Refresh (DateTime? fromDate = null, DateTime? toDate=null) {
            if (fromDate != null) FromDate=(DateTime) fromDate;
            if (toDate != null) ToDate=(DateTime) toDate;

            new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                _TOTAL_RECETTES.Content   =App.Store.Economat.Treasury.GetTotalRecette(FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
                _TOTAL_DEPENSES.Content   =(-App.Store.Economat.Treasury.GetTotalDepense(FromDate, ToDate)).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
                //_TOTAL_SCHOOL_FEE.Content =App.EStation.Economat.Treasury.GetTotalPaidSchoolFee(FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
                //_TOTAL_SALARIES.Content   =App.EStation.Economat.Treasury.GetTotalPaidSalaries(FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
                //_CAISSE_SOLDE.Content     =App.EStation.Economat.Treasury.GetSoldeCaisse(FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
                //_TOTAL_SOLDE.Content      =App.EStation.Economat.Treasury.GetSolde(FromDate, ToDate).ToString("0.##\\ dhs", CultureInfo.CurrentCulture);
            }))).Start();
        }


        private void DatePicker_OnSelectedDateChanged (object sender, SelectionChangedEventArgs e) {
           
            if((FromDate==_FROM_PICKER.SelectedDate.GetValueOrDefault()&&ToDate==_TO_PICKER.SelectedDate.GetValueOrDefault())||_TO_PICKER.SelectedDate==null)
                return;

            Refresh(_FROM_PICKER.SelectedDate.GetValueOrDefault(), _TO_PICKER.SelectedDate.GetValueOrDefault());
            DateSelectionChanged?.Invoke(null, e);
        }


       
    }
}
