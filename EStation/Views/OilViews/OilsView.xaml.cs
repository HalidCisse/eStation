using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



namespace EStation.Views.OilViews
{
    
    internal partial class OilsView 
    {
      
        public event EventHandler HuileSelectionChanged;

        public OilsView()
        {
            InitializeComponent();

            Refresh();
        }


        internal void Refresh()
            => new Task(() => Dispatcher.BeginInvoke(new Action(()
                => _HUILES.ItemsSource = App.EStation.Oils.GetOilsCards()))).Start();


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOil(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            => HuileSelectionChanged?.Invoke(_HUILES.SelectedValue, EventArgs.Empty);
       

    }
}
