using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using eStationCore.Model.Fuel.Entity;
using FirstFloor.ModernUI.Windows.Controls;

namespace eStation.Views.Fuel
{
    internal partial class AddPrice 
    {

        public AddPrice(Guid productGuid)
        {
            InitializeComponent();

            new Task(() =>
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    if (productGuid == Guid.Empty)
                    {
                        //ModernDialog.ShowMessage("Ajouter au moins un Carburant", "EStation", MessageBoxButton.OK);
                        Close();
                        return;
                    }

                    var productName =(await App.Store.Fuels.Get(productGuid)).Libel;

                    _TITLE_TEXT.Text = "NOUVEAU PRIX DE " + productName.ToUpper();

                   _GRID.DataContext = new Price { ProductGuid = productGuid,  FromDate = DateTime.Today, ActualPrice = 0};                   
                }));
            }).Start();
        }

    
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var newPrice = (Price)_GRID.DataContext;

                newPrice.FromDate = new DateTime(_START_DATE.SelectedDate.GetValueOrDefault().Year, _START_DATE.SelectedDate.GetValueOrDefault().Month,
                    _START_DATE.SelectedDate.GetValueOrDefault().Day, _START_TIME.Value.GetValueOrDefault().Hour, _START_TIME.Value.GetValueOrDefault().Minute, _START_TIME.Value.GetValueOrDefault().Second);

                App.Store.Pompes.Post(newPrice);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "ERREUR", MessageBoxButton.OK);
            }

            ModernDialog.ShowMessage("Enregistrer avec Success !" , "EStation", MessageBoxButton.OK);
            e.Handled = true;
            Close();
        }


        private void Annuler_Click(object sender, RoutedEventArgs e) => Close();


    }
}
