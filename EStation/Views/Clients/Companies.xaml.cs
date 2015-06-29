using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using eStation.Ext;
using eStationCore.Model.Sale.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Clients
{
    internal partial class Companies
    {

        public event EventHandler SelectionChanged;

        private List<Guid> SelectedCompanies => new List<Guid>(_COMPANIES.SelectedItems.Cast<CompanyCard>().Select(c => c.CompanyGuid));


        public Companies()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }


        internal async Task Refresh()
        {
            _BUSY_INDICATOR.IsBusy = true;
            _COMPANIES.ItemsSource = await App.Store.Sales.GetCompaniesCards();
            _COMPANIES.SelectAll();
            _BUSY_INDICATOR.IsBusy = false;
        }

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
            if (_COMPANIES.SelectedValue == null) return;

            var wind = new AddCompany((Guid)_COMPANIES.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private async void DeleteCompany_OnClick(object sender, RoutedEventArgs e)
        {
            if (_COMPANIES.SelectedItem == null) return;

            try
            {
                var card = ((CompanyCard)_COMPANIES.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer " + card.Name + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Sales.Delete(card.CompanyGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh();
            e.Handled = true;
        }
    }
}
