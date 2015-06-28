using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;
using eStationCore.Model.Sale.Enums;

namespace eStation.Views.Journals {
    

    public partial class FinanceDetails {

        
        public event EventHandler DateSelectionChanged;
       
        public DateTime FromDate { get; private set; }
     
        public DateTime ToDate { get; private set; }

     
        public FinanceDetails () {
            InitializeComponent();

        }

        public async Task Refresh (bool shouldInvoke = true, DateTime fromDate = default(DateTime), DateTime toDate = default(DateTime)) {

           await Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (fromDate != default(DateTime) && toDate != default(DateTime))
                {
                    _FROM_PICKER.SelectedDateChanged -= DatePicker_OnSelectedDateChanged;
                    _TO_PICKER.SelectedDateChanged -= DatePicker_OnSelectedDateChanged;

                    _FROM_PICKER.SelectedDate = FromDate = fromDate.AddDays(-1);
                    _TO_PICKER.SelectedDate = ToDate = toDate.AddDays(1);

                    _FROM_PICKER.SelectedDateChanged += DatePicker_OnSelectedDateChanged;
                    _TO_PICKER.SelectedDateChanged += DatePicker_OnSelectedDateChanged;
                }

                _TOTAL_RECETTES.Content = (await App.Store.Economat.Finance.GetTotalRecette(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
                _TOTAL_DEPENSES.Content = (-(await App.Store.Economat.Finance.GetTotalDepense(FromDate, ToDate))).ToString("C0", CultureInfo.CurrentCulture);

                _CAISSE_SOLDE.Content = (await App.Store.Economat.Finance.GetSoldeCaisse(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
                _TOTAL_SALARIES.Content = (await App.Store.Economat.Finance.GetTotalPaidSalaries(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

                _PAID_PURCHASE.Content = (await App.Store.Sales.GetPurchasedSum(null, PurchaseState.Paid, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
                _UNPAID_PURCHASE.Content = (await App.Store.Sales.GetPurchasedSum(null, PurchaseState.UnPaid, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

                _NOT_PAYING_PURCHASE.Content = (await App.Store.Sales.GetPurchasedSum(null, PurchaseState.NotPaying, FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);
                _TOTAL_SOLDE.Content = (await App.Store.Economat.Finance.GetRevenue(FromDate, ToDate)).ToString("C0", CultureInfo.CurrentCulture);

                if (shouldInvoke)
                    DateSelectionChanged?.Invoke(null, new EventArgs());
            }));                      
        }


        private async void DatePicker_OnSelectedDateChanged (object sender, SelectionChangedEventArgs e) {
           
            if(_TO_PICKER.SelectedDate == null || (FromDate==_FROM_PICKER.SelectedDate.GetValueOrDefault()&&ToDate==_TO_PICKER.SelectedDate.GetValueOrDefault()))
                return;
            await Refresh(true,_FROM_PICKER.SelectedDate.GetValueOrDefault(), _TO_PICKER.SelectedDate.GetValueOrDefault());
        }
       
    }
}
