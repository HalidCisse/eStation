using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EStation.Views.Clients
{
    internal partial class Purchases
    {
        private List<Guid> _companiesGuids=new List<Guid>();
        private DateTime _fromDate;
        private DateTime _toDate;


        public Purchases()
        {            
            InitializeComponent();
        }


        public async Task Refresh(List<Guid> companiesGuids, DateTime fromDate, DateTime toDate)
        {
            _companiesGuids = companiesGuids;
            _fromDate = fromDate;
            _toDate = toDate;
            
            _PURCHASES.ItemsSource = await App.Store.Sales.GetPurchasesCards(companiesGuids, fromDate, toDate); 

            _TITLE_TEXT.Text = "Bons de ";
            foreach (var companiesGuid in companiesGuids)
                _TITLE_TEXT.Text += (await App.Store.Sales.Get(companiesGuid)).Name + " ";
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_companiesGuids.Count == 0) return;

            Guid theCompany;

            if (_PURCHASES.SelectedValue != null)
                theCompany = (Guid) _PURCHASES.SelectedValue;
            else
                theCompany = (_companiesGuids).First();
           
            var wind = new AddPurchase(theCompany) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_companiesGuids, _fromDate, _toDate);
        }



    }
}
