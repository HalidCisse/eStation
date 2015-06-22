using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EStationCore.Model.Oil.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.OilViews
{
    
    internal partial class AddOilDelivery 
    {
        private bool _isAdd;

        public AddOilDelivery(Guid currentOil, Guid deliveryToMod)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    if (currentOil == Guid.Empty)
                    {
                        ModernDialog.ShowMessage("Selectionner une Huile", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }                   

                    //var vide = App.EStation.Oils.Get(currentOil).StockCapacity -
                    //           App.EStation.Oils.GetOilBalance(currentOil);
                   
                    //if (vide <= 0)
                    //{
                    //    ModernDialog.ShowMessage("Capaciter Maximale Attente", "EStation", MessageBoxButton.OK);
                    //    Close();
                    //    return;
                    //}

                    _SUPPLIER.ItemsSource = await App.Store.Citernes.GetSuppliers();

                    if (deliveryToMod == Guid.Empty)
                    {
                        _isAdd = true;

                        _GRID.DataContext = new OilDelivery
                        {
                            OilGuid = currentOil,
                            Supplier = ((string)_SUPPLIER.Items.GetItemAt(0)),
                            QuantityDelivered = 0,
                            DeliveryDate = DateTime.Now,
                            Cost = 0
                        };
                    }
                    else
                        _GRID.DataContext = App.Store.Oils.GetDelivery(deliveryToMod);

                    //_QUANTITY.Maximum = vide;
                    _TITLE_TEXT.Text = "LIVRAISON " + App.Store.Oils.Get(currentOil).Libel.ToUpper();
                }));
            }).Start();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) App.Store.Oils.Post((OilDelivery)_GRID.DataContext);
                else App.Store.Oils.Put((OilDelivery)_GRID.DataContext);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }

            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "EStation", MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();



    }
}
