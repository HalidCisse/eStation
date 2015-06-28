using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using eStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.FuelViews
{
    internal partial class AddFuelDelivery
    {
        private bool _isAdd;
       
        public AddFuelDelivery(Guid currentCiterne , Guid stockToMod)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    _SUPPLIER.ItemsSource = await App.Store.Citernes.GetSuppliers();

                    var vide = (await  App.Store.Citernes.Get(currentCiterne)).MaxCapacity -
                               (await App.Store.Citernes.GetCiterneFuelBalance(currentCiterne));

                    if (currentCiterne == Guid.Empty)
                    {
                        ModernDialog.ShowMessage("Selectionner une Citerne", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    if (vide <= 0)
                    {
                        ModernDialog.ShowMessage("Cette Citerne Est Pleine", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    if (stockToMod == Guid.Empty)
                    {
                        _isAdd = true;
                      
                        _GRID.DataContext = new FuelDelivery
                        {
                            CiterneGuid = currentCiterne,
                            Supplier = ((string)_SUPPLIER.Items.GetItemAt(0)),
                            QuantityDelivered = 0,
                            DeliveryDate = DateTime.Now,
                            Cost = 0
                        };                        
                    }
                    else
                        _GRID.DataContext = await App.Store.Citernes.GetStock(stockToMod);
                   
                    _QUANTITY.Maximum = vide;
                    _TITLE_TEXT.Text = "LIVRAISON " + (await App.Store.Citernes.Get(currentCiterne)).Libel.ToUpper();
                }));
            }).Start();
        }

        private async void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) {await App.Store.Citernes.Post((FuelDelivery)_GRID.DataContext);}
                else {await App.Store.Citernes.Put((FuelDelivery)_GRID.DataContext);}
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
            e.CanExecute =true;
            e.Handled = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();


    }
}
