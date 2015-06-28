using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using eStationCore.Model.Sale.Entity;
using eStationCore.Model.Sale.Enums;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Clients
{
    
    internal partial class AddPurchaseService
    {
        private int _errors;

        public AddPurchaseService(Guid companyGuid = default(Guid))
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (companyGuid == default(Guid))
                {
                    ModernDialog.ShowMessage("Choisir un client", "eStation", MessageBoxButton.OK);
                    Close();
                    return;
                }

                _STATUS.ItemsSource = EnumsHelper.GetAllValuesAndDescriptions<PurchaseState>();
             
                _GRID.DataContext = new Purchase
                {
                    Sum = 0,
                    ProductType = ProductType.Service,
                    PurchaseDate = DateTime.Now,
                    PurchaseState = PurchaseState.UnPaid,
                    CompanyGuid = companyGuid
                };
                _TITLE_TEXT.Text = $"Bon de {(await App.Store.Sales.Get(companyGuid))?.Name}";
            }));
        }

        private async void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if ((await App.Store.Sales.Post((Purchase)_GRID.DataContext)))
                    ModernDialog.ShowMessage("Enregistrer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled = true;
                return;
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }

            e.Handled = true;
            Close();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors==0;
            e.Handled = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }
    }
}
