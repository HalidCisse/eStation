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

                    var curPomp = App.Store.Pompes.Get(currentPompe);
                    if (curPomp.CiterneGuid != null)
                    {
                        var citerneBalance = App.Store.Citernes.GetCiterneFuelBalance((Guid) curPomp.CiterneGuid);

                        if (citerneBalance<=0)
                        {
                            ModernDialog.ShowMessage("Il n'ya pas de carburant dans le Citerne", "EStation", MessageBoxButton.OK);
                            Close();
                            return;
                        }

                        _TITLE_TEXT.Text = "Prélèvement ".ToUpper() + curPomp.Libel.ToUpper();
                        _derPrelev = App.Store.Pompes.GetLastPrelevement(currentPompe);
                        _COMPTEUR_M.Minimum = _derPrelev.Meter;
                        _COMPTEUR_M.Maximum = _derPrelev.Meter + citerneBalance;
                        _COMPTEUR_E.Maximum = citerneBalance;
                    }
                    else
                    {
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
                            Meter = _derPrelev.Meter,
                            MeterE = 0
                        };
                    }
                    else
                        _GRID.DataContext = App.Store.Pompes.GetPrelevement(prelevToMod);

                                   
                }));
            }).Start();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (_isAdd) App.Store.Pompes.Post((Prelevement)_GRID.DataContext);
                else App.Store.Pompes.Put((Prelevement)_GRID.DataContext);
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
