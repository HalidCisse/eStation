using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStationCore.Model.Sale.Entity;
using EStationCore.Model.Sale.Enums;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Clients
{
    

    internal partial class AddPurchase 
    {
        private double _curFuelPrice;


        public AddPurchase(Guid companyGuid = default(Guid))
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (companyGuid == Guid.Empty)
                {
                    ModernDialog.ShowMessage("Choisir un client", "eStation", MessageBoxButton.OK);
                    Close();
                    return;
                }

                _STATUS.ItemsSource = EnumsHelper.GetAllValuesAndDescriptions<PurchaseState>();
                _CARBURANT.ItemsSource = (await App.Store.Fuels.GetFuels()).ToDictionary(c => c.Libel, f=> f.FuelGuid);

                if (_CARBURANT.Items.Count == 0)
                {
                    ModernDialog.ShowMessage("Ajouter au moins un carburant dans stock", "eStation", MessageBoxButton.OK);
                    Close();
                    return;
                }

                _GRID.DataContext = new Purchase
                {
                    ProductType = ProductType.Fuel,
                    ProductGuid = ((KeyValuePair<string, Guid>)_CARBURANT.Items.GetItemAt(0)).Value,
                    PurchaseDate = DateTime.Now,
                    CompanyGuid = companyGuid
                };
                _TITLE_TEXT.Text = $"Bon de {(await App.Store.Sales.Get(companyGuid)).Name}";
                
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
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();

        private void UpDownBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_CARBURANT.SelectedValue ==null) return;          
            _MONTANT.Text = (_QUANTITY.Value.GetValueOrDefault()*_curFuelPrice).ToString("0.##") + " DH";
        }

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_CARBURANT.SelectedValue == null) return;
            _curFuelPrice = await App.Store.Fuels.GetFuelActualPrice((Guid) _CARBURANT.SelectedValue);
            _MONTANT.Text = (_QUANTITY.Value.GetValueOrDefault()*_curFuelPrice).ToString("0.##") + " DH";
        }

    }
}
