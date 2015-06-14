using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EStationCore.Model.Oil.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.OilViews
{
    
    internal partial class AddOil 
    {      
        private bool _isAdd;
        private int _errors;


        public AddOil(Guid oilToModGuid)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _OIL_TYPE.ItemsSource = App.Store.Oils.GetTypes();

                    if (oilToModGuid == Guid.Empty)
                    {
                        _isAdd = true;

                        _GRID.DataContext = new Oil
                        {
                            LiterPerGallon = 4,
                            Threshold = 10,
                            CurrentUnitPrice = 1,
                        };
                    }
                    else
                        _GRID.DataContext = App.Store.Oils.Get(oilToModGuid);
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
                if (_isAdd) App.Store.Oils.Post((Oil)_GRID.DataContext);
                else App.Store.Oils.Put((Oil)_GRID.DataContext);
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
