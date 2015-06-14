using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLib;
using EStation.Ext;
using EStationCore.Model.Hr.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Hr
{
    
    /// <summary>
    /// 
    /// </summary>
    public partial class StaffsView
    {        
        private bool _isFirstTime = true;

        /// <summary>
        /// 
        /// </summary>
        public StaffsView ( ) {

            InitializeComponent ();
        }


        private void UpdateData()
        {
            _BUSY_INDICATOR.IsBusy = true;

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {                  
                    _STAFF_LIST.ItemsSource = App.Store.HumanResource.GetDepStaffsCard();
                    _BUSY_INDICATOR.IsBusy = false;
                }));
            }).Start();

            _isFirstTime = true;
        }


        private void AddButon_Click (object sender, RoutedEventArgs e) {
            _BUSY_INDICATOR.IsBusy=false;
            var wind = new AddStaff(Guid.Empty) { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
            UpdateData();
        }


        private void BACK_BUTTON_OnClick (object sender, RoutedEventArgs e) {
            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }));
            }).Start();
        }


        private void Page_Loaded ( object sender, RoutedEventArgs e )
        {           
            UpdateData();            
        }

       
        private void DepStaffList_SelectionChanged ( object sender, SelectionChangedEventArgs e )
        {
            var staff = sender as ListBox;

            if(staff == null) return;

            if(staff.SelectedValue == null)
            {
                //_currentSelected = null;
            }
            //_currentSelected = staff.SelectedValue.ToString ();
        }
       

        private void DepStaffList_MouseDoubleClick ( object sender, MouseButtonEventArgs e )
        {
            var staffs = sender as ListBox;
            if(staffs?.SelectedValue==null)
                return;

            var staffObject = (Staff)staffs.SelectedItem;

            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() =>{NavigationService?.Navigate(new StaffDetails(staffObject));}));}).Start();
        }
 

        private void DepStaffList_Loaded ( object sender, RoutedEventArgs e )
        {
            if (!_isFirstTime) return;
           
            var eX = FindVisual.FindVisualChildren<Expander>(this).First();

            if (eX != null) eX.IsExpanded = true;
            _isFirstTime = false;                     
        }
      

        private void Expander_Expanded ( object sender, RoutedEventArgs e )
        {
            var eX = sender as Expander;
                     
            foreach(var ep in FindVisual.FindVisualChildren<Expander>(this).Where(ep => eX != null && ep.Header?.ToString() != eX.Header?.ToString()))
            {
                ep.IsExpanded = false;                
            }
        }


        private void Modifier_OnClick (object sender, RoutedEventArgs e) {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if(list?.SelectedValue==null)
                return;

            _BUSY_INDICATOR.IsBusy=false;
            var wind = new AddStaff((Guid) list.SelectedValue) { Owner=Window.GetWindow(this) };
            wind.ShowDialog();
            UpdateData();
        }


        private void Archiver_OnClick (object sender, RoutedEventArgs e) {

            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if(list?.SelectedValue==null)
                return;

            var dialog = new ModernDialog {
                Title="EStation",
                Content="Ete vous sure de supprimer "+ ((Staff)list.SelectedItem).Person.FullName +" de la base de donneé"
            };

            if(dialog.ShowDialogOkCancel()!=MessageBoxResult.OK)
                return;

            try
            {
                App.Store.HumanResource.DeleteStaff((Guid) list.SelectedValue);
            }
            catch (SecurityException) {
                ModernDialog.ShowMessage("Permission Refusée", "ERREUR", MessageBoxButton.OK);
                return;
            } catch (Exception ex) {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }
            ModernDialog.ShowMessage("Archiver avec Success !", "EStation",
                MessageBoxButton.OK);
        }


        private void Details_Click (object sender, RoutedEventArgs e) {

            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if(list?.SelectedItem==null) return;

            var staffObject = (Staff)list.SelectedItem;

            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => { NavigationService?.Navigate(new StaffDetails(staffObject)); }));
            }).Start();
        }


    }
}
