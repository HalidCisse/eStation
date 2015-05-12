using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Security {
    
    internal partial class ChangePass {
        
        public ChangePass () {
            InitializeComponent();
        }

        private void Save_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = _NEW_PASS.Password.Equals(_NEW_PASS_CONF.Password) && !string.IsNullOrEmpty(_LOGIN.Text.Trim());
            e.Handled=true;
        }

        private void save_Executed (object sender, ExecutedRoutedEventArgs e) {
            bool status;
            try {                
                status = App.EStation.Authentication.ChangePassWord(_LOGIN.Text.Trim(), _PASS.Password, _NEW_PASS.Password);                
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled=true;
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            if(status!=true) {
                ModernDialog.ShowMessage("La réinitialisation du Mot de passe a échoué. Se il vous plaît entrer de nouveau vos valeurs et essayez à nouveau", "ESchool",
                MessageBoxButton.OK);
                return;
            }

            ModernDialog.ShowMessage("Succès", "ERREUR",
                MessageBoxButton.OK);
            e.Handled=true;
            Close();
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) => Close();

       
    }
}
