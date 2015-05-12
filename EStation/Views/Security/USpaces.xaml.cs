
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
    internal partial class USpaces {

        public USpaces () {
            InitializeComponent();
        }

        public event EventHandler SpaceChanged;

        private Guid _profileGuid;

        public Guid ProfileGuid {
            set {
                new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                    _profileGuid=value;
                    _ESPACES_LIST.ItemsSource=App.EStation.Authentication.UserSpaces(_profileGuid);
                }))).Start();
            }
            get { return _profileGuid; }
        }
     
        private void IsInEspaceCheck_OnClick(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if(checkBox==null)
                return;

            try {
                if(checkBox.IsChecked!=null)
                    App.EStation.Authentication.SpaceClearance(((ViewCard)checkBox.DataContext).Info2.ToEnum<UserSpace>(), (bool)checkBox.IsChecked, _profileGuid);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
            } catch (Exception ex) {
                DebugHelper.WriteException(ex);
            }
            ProfileGuid=_profileGuid;
        }

        private void _ESPACES_LIST_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_ESPACES_LIST.SelectedValue == null) return;
            
            SpaceChanged?.Invoke(_ESPACES_LIST.SelectedValue.ToString().ToEnum<UserSpace>(), new EventArgs());
        }


    }
}
