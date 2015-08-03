using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using eLib;
using eStation.Views.Common;
using eStation.Views.Security;
using eStationCore.Model.Security.Enums;

namespace eStation
{
   
    internal partial class Login
    {
        
        public Login()
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow == null) return;
                    //mainWindow._SETTING_BUTTON.Visibility = Visibility.Hidden;
                    mainWindow._USER_NAME_LABEL.Visibility = Visibility.Collapsed;
                    mainWindow._POPUP_BUTTON.Visibility = Visibility.Collapsed;
                    Keyboard.Focus(_PSEUDO);
                    #if (DEBUG)
                    {
                        _PSEUDO.Text = "admin";   //"halid";
                        _PASSWORD.Password = "012345."; //"halid5.";
                    }
                    #endif                   
                }));
            }).Start();
        
        }


        private void _LOGIN_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_PSEUDO.Text.Trim()) || _PSEUDO.Text.Equals("Pseudo")) { Keyboard.Focus(_PSEUDO); return; }
            if (string.IsNullOrEmpty(_PASSWORD.Password)) { Keyboard.Focus(_PASSWORD); return; }

            Authenticate(_PSEUDO.Text.Trim(), _PASSWORD.Password);
        }


        private void Authenticate(string userName, string pass) {
            try
            {
                _BUSY_INDICATOR.IsBusy = true;
                if (App.Store.Authentication.Authenticate(userName, pass))
                {
                    var userKey = Membership.GetUser()?.ProviderUserKey;
                    if (userKey != null)
                        App.CurrentUser = App.Store.Authentication.GetUser((Guid) userKey);

                    var userSpace = (UserSpace) App.CurrentUser.UserSpaces.First().Value;

                    switch (userSpace)
                    {
                        case UserSpace.AdminSpace:
                            Dispatcher.BeginInvoke(new Action(()
                                => NavigationService?.Navigate(new HomePage(), UriKind.Relative)));
                            break;
                        //case UserSpace.SecretaireSpace:
                        //    new Task(() => Dispatcher.BeginInvoke(new Action(()
                        //        => NavigationService?.Navigate(new TeacherSpace(), UriKind.Relative)))).Start();
                        //    break;
                        //case UserSpace.EconomatSpace:
                        //    new Task(() => Dispatcher.BeginInvoke(new Action(()
                        //        => NavigationService?.Navigate(new EconomatView(false), UriKind.Relative)))).Start();
                        //    break;
                        default:
                            _ERROR_LABEL.Visibility = Visibility.Visible;
                            _ERROR_LABEL.Text = "Oops! Espace utilisateur non Implementer";
                            break;
                    }
                    _BUSY_INDICATOR.IsBusy = false;
                }
                else
                {
                    _BUSY_INDICATOR.IsBusy = false;
                    _ERROR_LABEL.Visibility = Visibility.Visible;
                    _ERROR_LABEL.Text = "Pseudo / mot de passe incorrect / ou compte Inactif";
                }
            }
            catch (SecurityException)
            {
                _ERROR_LABEL.Visibility = Visibility.Visible;
                _ERROR_LABEL.Text = "Permission Refusée, Votre Compte est bloqué !";
                _BUSY_INDICATOR.IsBusy = false;
            }
            catch (SqlException sqlException)
            {
                _ERROR_LABEL.Visibility = Visibility.Visible;
                _ERROR_LABEL.Text = "Connection au serveur a echoué, Veiller configurer le Sql serveur!";
                _ERROR_LABEL.ToolTip = sqlException.Message;
                _BUSY_INDICATOR.IsBusy = false;
            }
            catch (Exception exx)
            {
                DebugHelper.WriteException(exx);
                _ERROR_LABEL.Visibility = Visibility.Visible;
                _ERROR_LABEL.Text = "Erreur Inconnue !";
                _ERROR_LABEL.ToolTip = exx.Message;
                _BUSY_INDICATOR.IsBusy = false;
            }
            finally
            {
                _BUSY_INDICATOR.IsBusy = false;
            }
        }


        private void _PSEUDO_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    _BUSY_INDICATOR.IsBusy = true;
            //    var img =  App.Store.Authentication.GetUserPic(_PSEUDO.Text);
            //    _USER_IMAGE.DataContext = img ?? ImagesHelper.ImageToByteArray(Properties.Resources.mainicon);
            //    _BUSY_INDICATOR.IsBusy = false;
            //}
            //catch (Exception exception)
            //{
            //    DebugHelper.WriteException(exception);
            //}
        }


        private void _PSEUDO_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            

            //_BUSY_INDICATOR.IsBusy=true;

            //var work = Task<byte[]>.Factory.StartNew(GetPic);
            //await work;

            ////var img = await Task.FromResult(App.DataS.Authentication.GetUserPic(_PSEUDO.Text));
            //_USER_IMAGE.DataContext=work.Result ?? ImagesHelper.ImageToByteArray(Properties.Resources.mainicon);
            //_BUSY_INDICATOR.IsBusy=false;


        }


        private void ResetPasse_Click (object sender, RoutedEventArgs e) {

            var wind = new ResetPass(_PSEUDO.Text.Trim()) { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
        }


        //private byte[] GetPic ()
        //{
        //    byte[] result = {};           
        //    Dispatcher.Invoke(new Action(() => result = App.DataS.Authentication.GetUserPic(_PSEUDO.Text)));
        //    return result;
        //}
    }
}
