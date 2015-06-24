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
        private double _curPrice;
        private readonly bool _isFuel;


        public AddPurchase(Guid companyGuid = default(Guid), bool isFuel = true)
        {
            _isFuel = isFuel;
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

                if (isFuel)
                    _PRODUCT.ItemsSource = (await App.Store.Fuels.GetFuels()).ToDictionary(c => c.Libel, f => f.FuelGuid);
                else
                {
                    _PRODUCT_NAME.Text = "Huile";
                    _QUANTITY_LABEL.Text = "Quantité (bidons)";
                    _PRODUCT.ItemsSource = (await App.Store.Oils.GetOils()).ToDictionary(c => c.Libel, f => f.OilGuid);
                }

                if (_PRODUCT.Items.Count == 0)
                {
                    ModernDialog.ShowMessage(isFuel ? "Ajouter au moins un carburant dans stock" : "Ajouter au moins un huile dans stock", "eStation", MessageBoxButton.OK);
                    Close();
                    return;
                }

                _GRID.DataContext = new Purchase
                {
                    ProductType = isFuel ? ProductType.Fuel : ProductType.Oil,
                    ProductGuid = ((KeyValuePair<string, Guid>)_PRODUCT.Items.GetItemAt(0)).Value,
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
            if (_PRODUCT.SelectedValue ==null) return;          
            _MONTANT.Text = (_QUANTITY.Value.GetValueOrDefault()*_curPrice).ToString("0.##") + " DH";
        }

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_PRODUCT.SelectedValue == null) return;
            _curPrice = _isFuel?  await App.Store.Fuels.GetFuelActualPrice((Guid) _PRODUCT.SelectedValue) : (await App.Store.Oils.Get((Guid)_PRODUCT.SelectedValue)).CurrentUnitPrice;
            _MONTANT.Text = (_QUANTITY.Value.GetValueOrDefault()*_curPrice).ToString("0.##") + " DH";
        }

    }
}
