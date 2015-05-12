using System;
using System.Security;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStationCore.Helpers;
using EStationCore.Model.Security.Entity;
using EStationCore.Model.Security.Enums;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Security
{
    

    internal partial class AddUser {
        private int _errors;
        private bool _isAdd;

        public AddUser (Guid profileGuid) {
            InitializeComponent();

            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => {

                    if(profileGuid==Guid.Empty) {
                        ModernDialog.ShowMessage("Profile non valide", "ESchool", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    _USER_SPACE.ItemsSource =EnumsHelper.GetAllValuesAndDescriptions<UserSpace>();
                    var membUser            = App.EStation.Authentication.GetUser(profileGuid);
                    var theStaff            = App.EStation.HumanResource.GetStaff(profileGuid);

                    if(membUser==null) {
                        _isAdd=true;

                        var newData = new User {
                            UserGuid         =profileGuid,
                            UserName         =theStaff.Person.EmailAdress,
                            EmailAdress      =theStaff.Person.EmailAdress,
                            PasswordQuestion = "",
                            PasswordAnswer   = ""
                        };
                        _GRID.DataContext=newData;
                    }
                    else {
                        var oldData = App.EStation.Authentication.GetUser(profileGuid);
                        _GRID.DataContext=oldData;

                        _USER_NAME.IsEnabled  =false;
                        _USER_SPACE.IsEnabled =false;
                    }
                }));
            }).Start();

        }

        private void save_Executed (object sender, ExecutedRoutedEventArgs e) {
            var status = MembershipCreateStatus.Success;
            try {
                var newData = (User)_GRID.DataContext;
                newData.Password = _PASS.Password;

                if (_isAdd)
                    status = App.EStation.Authentication.AddUser(newData);
                else
                    App.EStation.Authentication.UpdateUser(newData);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled=true;
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            if (status != MembershipCreateStatus.Success)
            {
                ModernDialog.ShowMessage(status.GetErrorDescription(), "ESchool",
                MessageBoxButton.OK);
                return;
            }

            ModernDialog.ShowMessage(status.GetErrorDescription(), "ERREUR",
                MessageBoxButton.OK);
            e.Handled=true;
            Close();
        }


        private void Save_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute=_errors==0;
            e.Handled=true;
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) => Close();


        private void Validation_Error (object sender, ValidationErrorEventArgs e) {
            if(e.Action==ValidationErrorEventAction.Added)
                _errors++;
            else
                _errors--;
        }


    }
}
