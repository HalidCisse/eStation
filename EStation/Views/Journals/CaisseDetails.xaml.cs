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


        /// <summary>
        /// Fetch Data
        /// </summary>
        public void Refresh (DateTime fromDate, DateTime toDate) {
            FromDate=fromDate;
            ToDate=toDate;

            new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                _TOTAL_RECETTES.Content   =App.EStation.Economat.Treasury.GetTotalRecette(FromDate, ToDate).ToString("0.##", CultureInfo.CurrentCulture)+" dh";
                _TOTAL_DEPENSES.Content   =(-App.EStation.Economat.Treasury.GetTotalDepense(FromDate, ToDate)).ToString("0.##", CultureInfo.CurrentCulture)+" dh";
                _TOTAL_SCHOOL_FEE.Content =App.EStation.Economat.Treasury.GetTotalPaidSchoolFee(FromDate, ToDate).ToString("0.##", CultureInfo.CurrentCulture)+" dh";
                _TOTAL_SALARIES.Content   =App.EStation.Economat.Treasury.GetTotalPaidSalaries(FromDate, ToDate).ToString("0.##", CultureInfo.CurrentCulture)+" dh";
                _CAISSE_SOLDE.Content     =App.EStation.Economat.Treasury.GetSoldeCaisse(FromDate, ToDate).ToString("0.##", CultureInfo.CurrentCulture)+" dh";
                _TOTAL_SOLDE.Content      =App.EStation.Economat.Treasury.GetSolde(FromDate, ToDate).ToString("0.##", CultureInfo.CurrentCulture)+" dh";
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
