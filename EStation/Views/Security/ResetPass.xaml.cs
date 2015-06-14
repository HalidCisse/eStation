using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Security
{
    

    internal partial class ResetPass {
        public ResetPass (string userName) {
            InitializeComponent();

            _LOGIN.Text = userName;
        }


        private void Save_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute=!string.IsNullOrEmpty(_QUESTION.Content?.ToString());
            e.Handled=true;
        }


        private void save_Executed (object sender, ExecutedRoutedEventArgs e) {
            string status;
            try {
                status=App.Store.Authentication.ResetPassword(_LOGIN.Text.Trim(), _REPONSE.Text);
            } catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                e.Handled=true;
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            if(string.IsNullOrEmpty(status)) {
                ModernDialog.ShowMessage("La réinitialisation du Mot de passe a échoué. Se il vous plaît entrer de nouveau vos valeurs et essayez à nouveau", "ESchool",
                MessageBoxButton.OK);
                return;
            }

            ModernDialog.ShowMessage("Succès \nVotre nouveau mot de passe est : " +status, "ERREUR",
                MessageBoxButton.OK);
            e.Handled=true;
            Close();
        }


        private void Annuler_Click (object sender, RoutedEventArgs e) => Close();


        private void _LOGIN_OnLostFocus(object sender, RoutedEventArgs e) => _QUESTION.Content = App.Store.Authentication.GetUserQuestion(_LOGIN.Text.Trim());


    }
}
