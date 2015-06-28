using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CLib;
using eStation.Ext;
using eStationCore.Model.Sale.Enums;
using eStationCore.Model.Sale.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Clients
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


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var cm = FindResource("AddContext") as ContextMenu;
            if (cm == null) return;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;      
        }

        private async void AddPurchaseFuel_Click(object sender, RoutedEventArgs e)
        {
            if (_companiesGuids.Count == 0) return;

            Guid theCompany;

            if (_PURCHASES.SelectedValue != null)
                theCompany = (Guid)_PURCHASES.SelectedValue;
            else
                theCompany = (_companiesGuids).First();

            var wind = new AddPurchase(theCompany) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_companiesGuids, _fromDate, _toDate);
        }

        private async void AddPurchaseOil_Click(object sender, RoutedEventArgs e)
        {
            if (_companiesGuids.Count == 0) return;

            Guid theCompany;

            if (_PURCHASES.SelectedValue != null)
                theCompany = (Guid)_PURCHASES.SelectedValue;
            else
                theCompany = (_companiesGuids).First();

            var wind = new AddPurchase(theCompany, false) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_companiesGuids, _fromDate, _toDate);
        }

        private async void AddPurchaseService_Click(object sender, RoutedEventArgs e)
        {
            if (_companiesGuids.Count == 0) return;

            Guid theCompany;

            if (_PURCHASES.SelectedValue != null)
                theCompany = (Guid)_PURCHASES.SelectedValue;
            else
                theCompany = (_companiesGuids).First();

            var wind = new AddPurchaseService(theCompany) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_companiesGuids, _fromDate, _toDate);
        }

        private void PayContext_OnOpened(object sender, RoutedEventArgs e)
        {
            if (_PURCHASES.SelectedValue == null) return;
            switch (((PurchaseCard)_PURCHASES.SelectedItem).PurchaseState)
            {
                case PurchaseState.NotPaying:
                    ((MenuItem)((ContextMenu)sender).Items.GetItemAt(0)).Visibility = Visibility.Collapsed;
                    ((MenuItem)((ContextMenu)sender).Items.GetItemAt(1)).Visibility = Visibility.Collapsed;
                    return;
                case PurchaseState.UnPaid:
                    ((MenuItem)((ContextMenu)sender).Items.GetItemAt(0)).Visibility = Visibility.Visible;
                    ((MenuItem)((ContextMenu)sender).Items.GetItemAt(1)).Visibility = Visibility.Collapsed;
                    break;
                default:
                    ((MenuItem)((ContextMenu)sender).Items.GetItemAt(0)).Visibility = Visibility.Collapsed;
                    ((MenuItem)((ContextMenu)sender).Items.GetItemAt(1)).Visibility = Visibility.Visible;
                    break;
            }
        }

        private async void Paycheck_Click(object sender, RoutedEventArgs e)
        {

            if (_PURCHASES.SelectedValue == null) return;

            var payCard = ((PurchaseCard)_PURCHASES.SelectedItem);

            try
            {
                if (payCard.PurchaseState == PurchaseState.UnPaid)
                    await App.Store.Sales.CheckOut(payCard.PurchaseGuid);
                else
                    await App.Store.Sales.CheckOut(payCard.PurchaseGuid, PurchaseState.UnPaid);
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            
            if (payCard.PurchaseState == PurchaseState.Paid)
                ModernDialog.ShowMessage("Annuler avec Succès !", "eStation", MessageBoxButton.OK);

           await Refresh( _companiesGuids, _fromDate, _toDate);
        }

        private async void DeletePay_Click(object sender, RoutedEventArgs e)
        {
            if (_PURCHASES.SelectedValue == null || _PURCHASES.SelectedItem == null) return;
           
            try
            {
                var payCard = ((PurchaseCard)_PURCHASES.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de Supprimer cet Bon de " + payCard.Company + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Sales.DeletePurchase(payCard.PurchaseGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh(_companiesGuids, _fromDate, _toDate);            
            e.Handled = true;
        }

    }
}
