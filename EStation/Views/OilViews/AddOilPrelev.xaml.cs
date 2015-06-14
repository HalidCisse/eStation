using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EStationCore.Model.Common.Views;
using EStationCore.Model.Oil.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.OilViews
{
    internal partial class AddOilPrelev 
    {
       
        public AddOilPrelev()
        {
            InitializeComponent();

            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _TITLE_TEXT.Text = "Prélèvement Huiles";
                _START_DATE.SelectedDate = DateTime.Now;
                _PRELEVS.ItemsSource = App.Store.Oils.GetOils().Select(oil => new ViewCard{Guid = oil.OilGuid, Info1 = oil.Libel}).ToList();
            }))).Start();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                App.Store.Oils.Post(((List<ViewCard>)_PRELEVS.ItemsSource)
                    .Select(s=> new OilPrelevement {OilGuid = s.Guid, TotalStock = s.Int1}).ToList(), _START_DATE.SelectedDate.GetValueOrDefault());
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

        //private void UpDownBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //    => _COMPTEUR_E.Value = _COMPTEUR_M.Value - _derPrelev.Meter;

    }
}
