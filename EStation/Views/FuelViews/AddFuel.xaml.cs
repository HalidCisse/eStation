using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using eStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.FuelViews
{
    
    internal partial class AddFuel 
    {
        
        private bool _isAdd;
        private int _errors;


        public AddFuel(Guid fuelToModGuid)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (fuelToModGuid == Guid.Empty)
                    {
                        _isAdd = true;

                        _GRID.DataContext = new eStationCore.Model.Fuel.Entity.Fuel {Threshold = 10};
                    }
                    else
                        _GRID.DataContext = App.Store.Fuels.Get(fuelToModGuid);
                }));
            }).Start();
        }


        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }

        private async void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var newFuel =
                    ((eStationCore.Model.Fuel.Entity.Fuel) _GRID.DataContext);

                if (_UNIT_PRICE.Value != null)
                    newFuel.Prices.Add(new Price
                    {
                        ActualPrice = (double)_UNIT_PRICE.Value
                    });

                if (_isAdd) {await App.Store.Fuels.Post(newFuel);}
                else {await App.Store.Fuels.Put((eStationCore.Model.Fuel.Entity.Fuel)_GRID.DataContext);}
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "EStation",
                MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();


    }
}
