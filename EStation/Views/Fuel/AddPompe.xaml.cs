using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Fuel
{
    
    internal partial class AddPompe
    {
        private bool _isAdd;
        private int _errors;

        
        public AddPompe(Guid pompeToModGuid)
        {
            InitializeComponent();

            new Task(() =>
            {


                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _CITERNES.ItemsSource = App.EStation.Citernes.GetCiternes();

                    if (_CITERNES.Items.Count == 0)
                    {
                        ModernDialog.ShowMessage("Ajouter au moins une Citerne", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    if (pompeToModGuid == Guid.Empty)
                    {
                        _isAdd = true;

                        _GRID.DataContext = new Pompe
                        {
                            CiterneGuid = ((Citerne)_CITERNES.Items.GetItemAt(0)).CiterneGuid,
                            InitialCounter = 0,
                            PistolNumber = 1
                        };
                    }
                    else
                        _GRID.DataContext = App.EStation.Pompes.Get(pompeToModGuid);
                }));
            }).Start();
        }


        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) App.EStation.Pompes.Post((Pompe)_GRID.DataContext);
                else App.EStation.Pompes.Put((Pompe)_GRID.DataContext);
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

        


    }
}
