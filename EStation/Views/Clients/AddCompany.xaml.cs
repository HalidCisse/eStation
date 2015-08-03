using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using eLib.Database.Model;
using eStationCore.Model.Sale.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Clients
{
    internal partial class AddCompany
    {
        private bool _isAdd;
        private int _errors;


        public AddCompany(Guid companyToModGuid = default(Guid))
        {
            InitializeComponent();

           
               Dispatcher.BeginInvoke(new Action(async () =>
               {
                   if (companyToModGuid == Guid.Empty)
                   {
                       _isAdd = true;

                       var obj = new Company
                       {
                           Address = new Address(),
                       };
                       _GRID.DataContext = obj;
                   }
                   else
                        _GRID.DataContext = await App.Store.Sales.Get(companyToModGuid);
               }));
            
        }


        private async void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {              
                if (_isAdd)
                    await App.Store.Sales.Post((Company)_GRID.DataContext);
                else
                    await App.Store.Sales.Put((Company)_GRID.DataContext);
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled = true;
                return;
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "eStation",
                MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }


        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _errors == 0;
            e.Handled = true;
        }


        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }


        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();

        



    }
}
