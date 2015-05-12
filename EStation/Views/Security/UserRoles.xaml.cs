using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CLib;
using EStationCore.Model.Common.Views;
using EStationCore.Model.Security.Enums;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Security
{
    

    internal partial class UserRoles {

        public UserRoles () {
            InitializeComponent();
        }

        private Guid _profileGuid;
        private UserSpace _userSpace;
      
        internal void Refresh(Guid profileGuid, UserSpace userSpace)
        {
            new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                _profileGuid =profileGuid;
                _userSpace   =userSpace;
                _ROLES_LIST.ItemsSource=App.EStation.Authentication.GetUserClearances(_profileGuid, userSpace);
            }))).Start();
        }

        private void IsInRoleCheck_OnClick(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if(checkBox==null)return;  
                          
            try {
                if(checkBox.IsChecked != null)
                    App.EStation.Authentication.Clearance(((ViewCard)checkBox.DataContext).Info2, (bool)checkBox.IsChecked, _profileGuid);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
            } catch (Exception ex) {
                DebugHelper.WriteException(ex);
            }
            Refresh(_profileGuid, _userSpace);
        }

    }
}
