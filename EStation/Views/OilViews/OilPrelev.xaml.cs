using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EStation.Views.OilViews
{
    /// <summary>
    /// Interaction logic for OilPrelev.xaml
    /// </summary>
    internal partial class OilPrelev 
    {
        private Guid _currentOil;


        public OilPrelev()
        {
            InitializeComponent();
        }


        public void Refresh(Guid currentOil)
        {
            _currentOil = currentOil;
            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _PRELEVS.ItemsSource = App.EStation.Pompes.GetPrelevCards(fuelsGuids, fromDate, toDate);
                _PRELEVS.ItemsSource = App.EStation.Oils.GetPrelevCards(_currentOil);
                _TITLE_TEXT.Text = "LIVRAISONS " + App.EStation.Oils.Get(_currentOil)?.Libel.ToUpper();
            }))).Start();
        }

  
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddOilDelivery(_currentOil, Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh(_currentOil);
        }


    }
}
