

using System;
using System.Threading.Tasks;

namespace EStation.Views.Journals
{
   
    internal partial class JournalView 
    {
        public JournalView()
        {
            InitializeComponent();
        }



        private void BACK_BUTTON_OnClick(object sender, System.Windows.RoutedEventArgs e) => new Task(() => Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }))).Start();


        private void FuelPeriodCard_OnDateSelectionChanged(object sender, EventArgs e)
        {
            
        }


    }
}
