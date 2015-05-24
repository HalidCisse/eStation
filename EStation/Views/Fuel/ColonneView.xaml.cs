using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace EStation.Views.Fuel
{
    
    public partial class ColonneView 
    {
        public ColonneView()
        {
            InitializeComponent();
        }

        private void Refresh()
        {
            _BUSY_INDICATOR.IsBusy = true;

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _COLONNES_LIST.ItemsSource = App.EStation.Pompes.GetColonnesCard();
                    _BUSY_INDICATOR.IsBusy = false;
                }));
            }).Start();
        }

        private void ContextDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContextMod_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if (list?.SelectedValue == null) return;

            var wind = new AddPompe((Guid)list.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }
      
        private void AddButon_Click(object sender, RoutedEventArgs e)
        {            
            var wind = new AddPompe(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }

        private void Pomps_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list = sender as ListBox;
            if (list?.SelectedValue == null) return;

            //NavigationService?.Navigate(new ClassDetails(new Guid(list.SelectedValue.ToString())));
        }

        private void Pomps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e)
        {
            new Task(() => {
                Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }));
            }).Start();
        }

        private void ColonneView_OnLoaded(object sender, RoutedEventArgs e) => Refresh();


        private void ContextPrelev_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)e.Source;
            var menu = (ContextMenu)menuItem.Parent;
            var list = (ListBox)menu.PlacementTarget;
            if (list?.SelectedValue == null) return;

            var wind = new AddPrelevement((Guid)list.SelectedValue, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }
    }
}
