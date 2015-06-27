using System;
using System.Windows;
using System.Windows.Controls;
using EStation.Views.Fuel;

namespace EStation.Views.FuelViews
{
    
    internal partial class Citernes 
    {

        public event EventHandler CiterneSelectionChanged;

        public Citernes()
        {
            InitializeComponent();

            Refresh();
        }


        internal async void Refresh()           
                => _CITERNES.ItemsSource = await App.Store.Citernes.GetCiternesCards();


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddCiterne(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }


        protected virtual void OnCiterneSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) 
            => CiterneSelectionChanged?.Invoke(_CITERNES.SelectedValue, EventArgs.Empty);


    }
}
