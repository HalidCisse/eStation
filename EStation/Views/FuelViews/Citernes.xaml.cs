using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using eLib;
using eStation.Ext;
using eStation.Views.Fuel;
using eStationCore.Model.Fuel.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.FuelViews
{
    
    internal partial class Citernes 
    {

        public event EventHandler CiterneSelectionChanged;

        public Citernes()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () => await Refresh()));
        }


        internal async Task Refresh()
        {
            _BUSY_INDICATOR.IsBusy = true;
            _CITERNES.ItemsSource = await App.Store.Citernes.GetCiternesCards();
            _BUSY_INDICATOR.IsBusy = false;
        }


        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddCiterne(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh();
        }


        protected virtual void OnCiterneSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) 
            => CiterneSelectionChanged?.Invoke(_CITERNES.SelectedValue, EventArgs.Empty);

        private async void ContextDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_CITERNES.SelectedItem == null) return;

            try
            {
                var card = ((CiterneCard)_CITERNES.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer " + card.Libel + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Citernes.Delete(card.CiterneGuid))
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
