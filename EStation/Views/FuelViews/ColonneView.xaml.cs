using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using eLib;
using eStation.Ext;
using eStation.Views.Fuel;
using eStationCore.Model.Fuel.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.FuelViews
{
    
    public partial class ColonneView 
    {
        public ColonneView()
        {
            InitializeComponent();
        }

        private async Task Refresh()
        {
            _BUSY_INDICATOR.IsBusy = true;            
            _COLONNES_LIST.ItemsSource = await App.Store.Pompes.GetColonnesCard();
            _BUSY_INDICATOR.IsBusy = false;                
        }

       
        private async void ContextMod_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if (list?.SelectedValue == null) return;

            var wind = new AddPompe((Guid)list.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }
      
        private async void AddButon_Click(object sender, RoutedEventArgs e)
        {            
            var wind = new AddPompe(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private void Pomps_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Pomps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e) 
            => await  Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }));

        private async void ColonneView_OnLoaded(object sender, RoutedEventArgs e) => await Refresh();


        private async void ContextPrelev_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if (list?.SelectedValue == null) return;

            var wind = new AddPrelevement((Guid)list.SelectedValue, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if (list?.SelectedItem == null) return;

            try
            {
                var card = ((PompeCard)list.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer " + card.Libel + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Pompes.Delete(card.PompeGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh();
            e.Handled = true;
        }
    }
}
