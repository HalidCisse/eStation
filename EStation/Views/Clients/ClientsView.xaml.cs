using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Clients
{
    
    public partial class ClientsView 
    {
        public ClientsView()
        {
            InitializeComponent();

            Refresh();
        }

        private async void Refresh()
        {
            try
            {
                //var work = Task<IEnumerable<StudentCard>>.Factory.StartNew(App.DataS.Students.GetStudentCards);

                //_BUSY_INDICATOR.IsBusy = true;

                //await work;

                //_BUSY_INDICATOR.IsBusy = false;

                //_STUDENTSLIST.ItemsSource = work.Result;
            }
            catch (SecurityException)
            {
                ModernDialog.ShowMessage("Permission Refusée", "Non Authoriser", MessageBoxButton.OK);
            }
            catch (Exception exx)
            {
                DebugHelper.WriteException(exx);
                ModernDialog.ShowMessage("ERREUR", "OoOPS", MessageBoxButton.OK);
            }
        }

        private void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }));
            }).Start();
        }


        private void ClientList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        { //if (_STUDENTSLIST?.SelectedItem == null) return;

            //new Task(() => {
            //    Dispatcher.BeginInvoke(new Action(() => { NavigationService?.Navigate(new StudentDetails((StudentCard)_STUDENTSLIST.SelectedItem)); }));
            //}).Start();

        }


        private void AddButon_Click(object sender, RoutedEventArgs e)
        {
            var wind = new NewClient(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {

            //if (_STUDENTSLIST?.SelectedItem == null)
            //    return;

            //new Task(() => {
            //    Dispatcher.BeginInvoke(new Action(() => { NavigationService?.Navigate(new StudentDetails((StudentCard)_STUDENTSLIST.SelectedItem)); }));
            //}).Start();
        }

        private void Modifier_OnClick(object sender, RoutedEventArgs e)
        {
            //if (_STUDENTSLIST?.SelectedItem == null)
            //    return;

            //_BUSY_INDICATOR.IsBusy = false;
            //var wind = new AddStudent((Guid)_STUDENTSLIST.SelectedValue) { Owner = Window.GetWindow(this) };
            //wind.ShowDialog();
            //Refresh();
        }

        private void Archiver_OnClick(object sender, RoutedEventArgs e)
        {
            //if (_STUDENTSLIST?.SelectedItem == null)
            //    return;

            //var dialog = new ModernDialog
            //{
            //    Title = "ESchool",
            //    Content = "Ete vous sure de supprimer " + ((StudentCard)_STUDENTSLIST.SelectedItem).FullName + " de la base de donneé"
            //};

            //if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
            //    return;

            //try
            //{
            //    App.DataS.Students.DeleteStudent((Guid)_STUDENTSLIST.SelectedValue);
            //}
            //catch (SecurityException)
            //{
            //    ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            //    return;
            //}
            //ModernDialog.ShowMessage("Archiver avec Success !", "ESchool",
            //    MessageBoxButton.OK);
        }


        private void _SEARCH_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(_SEARCH_BOX.Text))
            //    return;

            //try
            //{
            //    _BUSY_INDICATOR.IsBusy = true;

            //    new Task(() =>
            //    {
            //        Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            _STUDENTSLIST.ItemsSource = App.DataS.Students.Search(_SEARCH_BOX.Text);
            //            _BUSY_INDICATOR.IsBusy = false;
            //        }));
            //    }).Start();
            //}
            //catch (Exception ex)
            //{
            //    DebugHelper.WriteException(ex);
            //}
        }


        private void _SEARCH_BOX_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(_SEARCH_BOX.Text))
            //    return;

            //try
            //{
            //    _BUSY_INDICATOR.IsBusy = true;

            //    new Task(() => {
            //        Dispatcher.BeginInvoke(new Action(() => {
            //            _STUDENTSLIST.ItemsSource = App.DataS.Students.Search(_SEARCH_BOX.Text);
            //            _BUSY_INDICATOR.IsBusy = false;
            //        }));
            //    }).Start();
            //}
            //catch (Exception ex)
            //{
            //    DebugHelper.WriteException(ex);
            //}
        }

        
    }
}
