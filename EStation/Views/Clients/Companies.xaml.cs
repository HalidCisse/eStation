using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EStationCore.Model.Sale.Views;

namespace EStation.Views.Clients
{
    internal partial class Companies
    {

        public event EventHandler SelectionChanged;

        private List<Guid> SelectedCompanies => new List<Guid>(_CUSTOMERS.SelectedItems.Cast<CompanyCard>().Select(c => c.CompanyGuid));


        public Companies()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }


        internal async Task Refresh()=> await Dispatcher.BeginInvoke(new Action(async () => {
            _CUSTOMERS.ItemsSource = await App.Store.Sales.GetCompaniesCards();
            _CUSTOMERS.SelectAll();
        }));

        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddCompany() { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private void _CUSTOMERS_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            => SelectionChanged?.Invoke(SelectedCompanies, EventArgs.Empty);

        private async void _CUSTOMERS_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_CUSTOMERS.SelectedValue == null) return;

            var wind = new AddCompany((Guid)_CUSTOMERS.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }


    }
}
