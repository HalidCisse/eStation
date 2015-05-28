using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Fuel
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

                        _GRID.DataContext = new EStationCore.Model.Fuel.Entity.Fuel {Threshold = 10};
                    }
                    else
                        _GRID.DataContext = App.EStation.Fuels.Get(fuelToModGuid);
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

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var newFuel =
                    ((EStationCore.Model.Fuel.Entity.Fuel) _GRID.DataContext);

                if (_UNIT_PRICE.Value != null)
                    newFuel.Prices.Add(new Price
                    {
                        ActualPrice = (double)_UNIT_PRICE.Value
                    });

                if (_isAdd) App.EStation.Fuels.Post(newFuel);
                else App.EStation.Fuels.Put((EStationCore.Model.Fuel.Entity.Fuel)_GRID.DataContext);
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
