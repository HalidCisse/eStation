using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using eLib;
using eStation.Ext;
using eStationCore.Model.Oil.Views;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.OilViews
{
    
    internal partial class OilPrelev 
    {
        private List<Guid> _currentOils;


        public OilPrelev()
        {
            InitializeComponent();
        }


        public async Task Refresh(List<Guid> oilsGuids)
        {
            _BUSY_INDICATOR.IsBusy = true;
            _currentOils = oilsGuids ;
           
            _PRELEVS.ItemsSource = await App.Store.Oils.GetPrelevCards(_currentOils, DateTime.Today.AddDays(-7), DateTime.Today);
            _TITLE_TEXT.Text = "PRELEVEMENTS (" ;

            foreach (var oilGuid in oilsGuids)
                _TITLE_TEXT.Text += $" {(await App.Store.Oils.Get(oilGuid))?.Libel.ToUpper()}";
            _TITLE_TEXT.Text += ")";
            _BUSY_INDICATOR.IsBusy = false;
        }
          
        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOilPrelev { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            await Refresh(_currentOils);
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_PRELEVS.SelectedItem == null) return;

            try
            {
                var card = ((OilPrelevCard)_PRELEVS.SelectedItem);

                var dialog = new ModernDialog
                {
                    Title = "eStation",
                    Content = "Ete vous sure de supprimer cet prelevement de " + card.Libel + " ?"
                };

                if (dialog.ShowDialogOkCancel() != MessageBoxResult.OK)
                    return;
                if (await App.Store.Oils.DeletePrelevement(card.OilPrelevementGuid))
                    ModernDialog.ShowMessage("Supprimer avec Success !", "eStation", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("Erreur Inconnue !", "eStation", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }
            await Refresh(_currentOils);
            e.Handled = true;
        }
    }
}
