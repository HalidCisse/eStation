using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CLib;
using CLib.Validation;
using EStationCore.Model.Common.Entity;
using EStationCore.Model.Common.Enums;
using EStationCore.Model.Customers.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Clients
{
   
    internal partial class NewClient 
    {
        private bool _isAdd;
        private int _errors;


        public NewClient(Guid clientToModGuid)
        {
            InitializeComponent();

            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => {

                    _CIVILITE_.ItemsSource = EnumsHelper.GetAllValuesAndDescriptions<PersonTitles>();
                    _STATUT.ItemsSource = EnumsHelper.GetAllValuesAndDescriptions<CustomerStatus>();

                    _NATIONALITY.ItemsSource = App.EStation.Customers.AllNationalities();

                    if (clientToModGuid == Guid.Empty)
                    {
                        _isAdd = true;

                        var obj = new Customer
                        {
                            Matricule = App.EStation.Customers.NewMatricule(),

                            CustomerStatus = CustomerStatus.Default,

                            Person = new Person
                            {
                                Title = PersonTitles.Mr,
                                HealthState = HealthStates.Normal,
                                BirthDate = DateTime.Today.AddYears(-20)
                            }                            
                        };
                        _GRID.DataContext = obj;
                    }
                    else
                    {
                        var data = App.EStation.Customers.GetCustomer(clientToModGuid);
                        _PHOTO_IDENTITY.Source = ImagesHelper.DecodePhoto(data.Person.PhotoIdentity);
                        _MATRICULE_VALIDATOR.IsAdd = false;
                        _GRID.DataContext = data;

                        _MATRICULE.IsEnabled = false;
                    }
                }));
            }).Start();
        }


        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var newData = (Customer)_GRID.DataContext;
                newData.Person.PhotoIdentity =
                    ImagesHelper.GetPngFromImageControl(_PHOTO_IDENTITY.Source as BitmapImage);

                if (_isAdd)
                    App.EStation.Customers.AddCustomer(newData);
                else
                    App.EStation.Customers.UpdateCustomer(newData);
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
            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "Winxo",
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


        private void PhotoID_Click(object sender, RoutedEventArgs e)
        {
            var photo = ImagesHelper.OpenImageFileDialog();
            if (photo != null)
                _PHOTO_IDENTITY.Source = new BitmapImage(new Uri(photo));
        }


        private void PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_errors == 0)
                ((TextBox)sender).Text = InputHelper.FormatNumber(((TextBox)sender).Text);
        }







    }
}
