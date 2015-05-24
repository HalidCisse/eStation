using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Fuel
{
    internal partial class AddFuelStock 
    {
        private bool _isAdd;
       
        public AddFuelStock(Guid currentCiterne , Guid stockToMod)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _SUPPLIER.ItemsSource = App.EStation.Citernes.GetSuppliers();

                    var vide = App.EStation.Citernes.Get(currentCiterne).MaxCapacity -
                               App.EStation.Citernes.GetCiterneFuelBalance(currentCiterne);

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
                      
                        _GRID.DataContext = new FuelStock
                        {
                            CiterneGuid = currentCiterne,
                            Supplier = ((string)_SUPPLIER.Items.GetItemAt(0)),
                            Quantity = 0,
                            DateIssued = DateTime.Now,
                            Cost = 0
                        };                        
                    }
                    else
                        _GRID.DataContext = App.EStation.Citernes.GetStock(stockToMod);
                   
                    _QUANTITY.Maximum = vide;
                    _TITLE_TEXT.Text = "LIVRAISON " + App.EStation.Citernes.Get(currentCiterne).Libel.ToUpper();
                }));
            }).Start();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) App.EStation.Citernes.Post((FuelStock)_GRID.DataContext);
                else App.EStation.Citernes.Put((FuelStock)_GRID.DataContext);
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
