using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;


namespace EStation.Views.Journals
{
    
    internal partial class AnalyticView 
    {
        public AnalyticView()
        {
            InitializeComponent();

            Task.Run(async () => await Refresh());
        }


        private async void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e)
            => await Dispatcher.BeginInvoke(new Action(() => { NavigationService?.GoBack(); }));


        private async Task Refresh() => await _FUEL_CHART.Refresh(new List<Guid>(), DateTime.Today.AddYears(-1), DateTime.Today)
            .ContinueWith(task =>  _OIL_CHART.Refresh(new List<Guid>(), DateTime.Today.AddYears(-1), DateTime.Today))
            .ContinueWith(task =>  _FINANCE_CHART.Refresh(new List<Guid>(), DateTime.Today.AddYears(-1), DateTime.Today));


    }
}
