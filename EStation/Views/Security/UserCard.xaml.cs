using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Security
{
   
    internal partial class UserCard {


        public UserCard () {
            InitializeComponent();
        }

       
        public event EventHandler SpaceChanged;

        private Guid _profileGuid;

        public Guid ProfileGuid {
            set {
                new Task(() => Dispatcher.BeginInvoke(new Action(() => {
                    _profileGuid=value;                  
                    _GRID.DataContext=App.EStation.Authentication.GetUser(_profileGuid);                   
                    SpaceChanged?.Invoke(_profileGuid, new EventArgs());

                    _CONF.Visibility = _GRID.DataContext!= null ? Visibility.Hidden : Visibility.Visible;
                }))).Start();
            }
            
            get { return _profileGuid; }
        }

        private void AddUser_Click (object sender, RoutedEventArgs e) {
            var wind = new AddUser(_profileGuid) { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
            ProfileGuid = _profileGuid;
        }

        private void IsApproved_OnClick (object sender, RoutedEventArgs e) {
            bool status;
            var isApproved = ((ToggleButton)sender).IsChecked;
            if(isApproved==null)
                return;
            try
            {
                //if(!(bool)isApproved && App.Winxo.Pedagogy.Study.HasAnyJob(_profileGuid)) {
                //    var dialog = new ModernDialog {
                //        Title="ESchool",
                //        Content="Ce Staff dispose des taches active ete vous sure de continuer ?"
                //    };

                //    if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                //    {
                //        ((ToggleButton) sender).IsChecked = true;
                //        return;
                //    }                       
                //}

                status = App.EStation.Authentication.IsApproved(_profileGuid, (bool) isApproved);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled=true;
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }

            if (!status)
            {
                ModernDialog.ShowMessage("Erreur Réessayer plus tard", "ERREUR",
                MessageBoxButton.OK);
                return;
            }

            e.Handled=true;
        }


    }
}
