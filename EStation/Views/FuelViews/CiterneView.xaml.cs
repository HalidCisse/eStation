using System;
using System.Threading.Tasks;
using System.Windows;


namespace EStation.Views.Fuel
{
   
    internal partial class CiterneView 
    {
        public CiterneView()
        {
            InitializeComponent();

            Refresh();
        }

        internal void Refresh() 
            => new Task(() => Dispatcher.BeginInvoke(new Action(() 
                => _CITERNES.ItemsSource = App.Store.Citernes.GetCiternesCards()))).Start();


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddCiterne(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }


    }
}
