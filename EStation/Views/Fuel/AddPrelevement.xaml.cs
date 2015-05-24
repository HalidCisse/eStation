using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace EStation.Views.Fuel
{
    internal partial class AddPrelevement 
    {
        private bool _isAdd;
        private Prelevement _derPrelev;

        public AddPrelevement(Guid currentPompe, Guid prelevToMod)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {                  
                    if (currentPompe == Guid.Empty)
                    {
                        ModernDialog.ShowMessage("Selectionner un Pistolet", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    if (prelevToMod == Guid.Empty)
                    {
                        _isAdd = true;

                        _GRID.DataContext = new Prelevement
                        {
                            PompeGuid = currentPompe,
                            DatePrelevement = DateTime.Now,
                            Meter = 0,
                            MeterE = 0
                        };
                    }
                    else
                        _GRID.DataContext = App.EStation.Pompes.GetPrelevement(prelevToMod);

                    _TITLE_TEXT.Text = "Prélèvement " + App.EStation.Pompes.Get(currentPompe).Libel;

                    _derPrelev = App.EStation.Pompes.GetLastPrelevement(currentPompe);
                    _COMPTEUR_M.Minimum = _derPrelev.Meter;
                }));
            }).Start();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) App.EStation.Pompes.Post((Prelevement)_GRID.DataContext);
                else App.EStation.Pompes.Put((Prelevement)_GRID.DataContext);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }

            ModernDialog.ShowMessage(_isAdd ? "Enregistrer avec Success !" : "Modifier avec Success !", "EStation", MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();

        private void UpDownBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) 
            => _COMPTEUR_E.Value = _COMPTEUR_M.Value-_derPrelev.Meter ;

    }
}
