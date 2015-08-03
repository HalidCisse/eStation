using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using eStationCore.Model.Common.Views;
using eStationCore.Model.Oil.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.OilViews
{
    internal partial class AddOilPrelev 
    {
       
        public AddOilPrelev()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                _TITLE_TEXT.Text = "Prélèvement Huiles";
                _START_DATE.SelectedDate = DateTime.Now;
                _PRELEVS.ItemsSource = (await App.Store.Oils.GetOils()).Select(oil => new ViewCard {Guid = oil.OilGuid, Info1 = oil.Libel, Int1 = 0});
            }));
        }

        private async void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {              
                await App.Store.Oils.Post(_PRELEVS.Items.Cast<ViewCard>().ToList()
                    .Select(s=> new OilPrelevement{OilGuid = s.Guid, Meter = s.Int1}).ToList(), _START_DATE.SelectedDate.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
                return;
            }

            ModernDialog.ShowMessage("Enregistrer avec Success !" , "EStation", MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();

    }
}
