using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;


namespace EStation.Views.Fuel
{
    internal partial class AddCiterne 
    {
        private bool _isAdd;
        private int _errors;

        public AddCiterne(Guid citerneToMod)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    _FUELS.ItemsSource = (await App.Store.Fuels.GetFuels());

                    if (_FUELS.Items.Count == 0)
                    {
                        ModernDialog.ShowMessage("Ajouter au moins un Carburant", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    if (citerneToMod == Guid.Empty)
                    {
                        _isAdd = true;

                        _GRID.DataContext = new Citerne { Threshold = 100, FuelGuid = ((EStationCore.Model.Fuel.Entity.Fuel)_FUELS.Items.GetItemAt(0)).FuelGuid, MaxCapacity = 1000};
                    }
                    else
                        _GRID.DataContext = App.Store.Citernes.Get(citerneToMod);
                }));
            }).Start();
        }


        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) App.Store.Citernes.Post((Citerne)_GRID.DataContext);
                else App.Store.Citernes.Put((Citerne)_GRID.DataContext);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }

            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "EStation", MessageBoxButton.OK);
            e.Handled = true;
            Close();
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
        
        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();

    }
}
