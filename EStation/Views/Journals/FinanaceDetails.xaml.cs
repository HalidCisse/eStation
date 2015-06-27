using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;
using EStationCore.Model.Sale.Enums;

namespace EStation.Views.Journals {
    

    public partial class FinanceDetails {

        
        public event EventHandler DateSelectionChanged;
       
        public DateTime FromDate { get; private set; }
     
        public DateTime ToDate { get; private set; }

     
        public FinanceDetails () {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                _FROM_PICKER.SelectedDate=FromDate=DateTime.Today.AddMonths(-1);
                _TO_PICKER.SelectedDate=ToDate=DateTime.Today;
                await Refresh(FromDate, ToDate);
                
            }));
        }

       
        public async Task Refresh (DateTime? fromDate = null, DateTime? toDate=null) {
            if (fromDate != null) FromDate=(DateTime) fromDate;
            if (toDate != null) ToDate=(DateTime) toDate;
           
                      
            _TOTAL_RECETTES.Content = (await App.Store.Economat.Finance.GetTotalRecette(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
            _TOTAL_DEPENSES.Content = (-(await App.Store.Economat.Finance.GetTotalDepense(FromDate, ToDate))).ToString("C0", CultureInfo.CurrentCulture);

            _CAISSE_SOLDE.Content = (await App.Store.Economat.Finance.GetSoldeCaisse(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
            _TOTAL_SALARIES.Content = (await App.Store.Economat.Finance.GetTotalPaidSalaries(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            _PAID_PURCHASE.Content = (await App.Store.Sales.GetPurchasedSum(null, PurchaseState.Paid, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
            _UNPAID_PURCHASE.Content = (await App.Store.Sales.GetPurchasedSum(null, PurchaseState.UnPaid, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            _NOT_PAYING_PURCHASE.Content = (await App.Store.Sales.GetPurchasedSum(null, PurchaseState.NotPaying, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
            _TOTAL_SOLDE.Content = (await App.Store.Economat.Finance.GetRevenue(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

            DateSelectionChanged?.Invoke(null, new EventArgs());           
        }


        private async void DatePicker_OnSelectedDateChanged (object sender, SelectionChangedEventArgs e) {
           
            if((FromDate==_FROM_PICKER.SelectedDate.GetValueOrDefault()&&ToDate==_TO_PICKER.SelectedDate.GetValueOrDefault())||_TO_PICKER.SelectedDate==null)
                return;
            await Refresh(_FROM_PICKER.SelectedDate.GetValueOrDefault(), _TO_PICKER.SelectedDate.GetValueOrDefault());
            DateSelectionChanged?.Invoke(null, e);
        }


       
    }
}
