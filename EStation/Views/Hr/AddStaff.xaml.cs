using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CLib;
using CLib.Validation;
using eStationCore.Model.Common.Entity;
using eStationCore.Model.Common.Enums;
using eStationCore.Model.Hr.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Hr
{
    
    internal partial class AddStaff {

        private bool _isAdd;
        private int _errors;

        /// <summary>
        /// 
        /// </summary>
        public AddStaff (Guid staffToModGuid) {
            InitializeComponent();

            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => {

                    _CIVILITE_.ItemsSource     =EnumsHelper.GetAllValuesAndDescriptions<PersonTitles>();
                    _STATUT.ItemsSource        =EnumsHelper.GetAllValuesAndDescriptions<StaffStatus>();
                    _HEALTH_STATUT.ItemsSource =EnumsHelper.GetAllValuesAndDescriptions<HealthStates>();

                    _NATIONALITY.ItemsSource   =App.Store.HumanResource.AllNationalities();
                    _BIRTH_PLACE.ItemsSource   =App.Store.HumanResource.AllBirthPlaces();                  
                    _QUALIFICATION.ItemsSource =App.Store.HumanResource.AllStaffQualifications();
                    _DEPARTEMENT.ItemsSource   =App.Store.HumanResource.AllDepartement();
                    _GRADE.ItemsSource         =App.Store.HumanResource.AllGrades();

                    if(staffToModGuid==Guid.Empty) {
                        _isAdd=true;

                        var data = new Staff {
                            Matricule=App.Store.HumanResource.GetNewStaffMatricule() ,
                            Experiences = 0,
                            Statut = StaffStatus.Actif,
                            Person = new Person
                            {
                                Title = PersonTitles.Mr,                                
                                HealthState = HealthStates.Normal,
                                BirthDate = DateTime.Today.AddYears(-20)

                            }                          
                        };
                        _GRID.DataContext=data;
                    }
                    else {
                        var data = App.Store.HumanResource.GetStaff(staffToModGuid);
                        _PHOTO_IDENTITY.Source=ImagesHelper.DecodePhoto(data.Person.PhotoIdentity);
                        _MATRICULE_VALIDATOR.IsAdd = false;
                        //_TITLE_TEXT.Text="MODIFICATION";
                        _GRID.DataContext=data;

                        _MATRICULE.IsEnabled=false;                       
                    }
                }));
            }).Start();
        }

        
        private void save_Executed (object sender, ExecutedRoutedEventArgs e) {
            try {
                var newStaff = (Staff)_GRID.DataContext;
                newStaff.Person.PhotoIdentity =
                    ImagesHelper.GetPngFromImageControl(_PHOTO_IDENTITY.Source as BitmapImage);              
                if(_isAdd)
                    App.Store.HumanResource.AddStaff(newStaff);
                else
                    App.Store.HumanResource.UpdateStaff(newStaff);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled=true;
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "ESchool",
                MessageBoxButton.OK);
            e.Handled=true;
            Close();
        }


        private void Validation_Error (object sender, ValidationErrorEventArgs e) {
            if(e.Action==ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }


        private void Save_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute=_errors==0;
            e.Handled=true;
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) => Close();


        private void PhotoID_Click (object sender, RoutedEventArgs e) {
            var photo = ImagesHelper.OpenImageFileDialog();
            if(photo!=null)
                _PHOTO_IDENTITY.Source=new BitmapImage(new Uri(photo));
        }


        private void PhoneNumber_TextChanged (object sender, TextChangedEventArgs e)
        {
            if (_errors==0)
                ((TextBox)sender).Text=InputHelper.FormatNumber(((TextBox)sender).Text);           
        }


    }
}
