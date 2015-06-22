using System;
using System.Collections.Generic;
using System.Windows;


namespace EStation.Views.Clients
{
   
    public partial class CompaniesView 
    {
        public CompaniesView()
        {
            InitializeComponent();
        }


        

        private async void BACK_BUTTON_OnClick(object sender, RoutedEventArgs e)
            => await Dispatcher.BeginInvoke(new Action(()
                => { NavigationService?.GoBack(); }));

        private async void COMPANIES_OnSelectionChanged(object sender, EventArgs e) 
            => await _PURCHASES.Refresh((List<Guid>) sender, DateTime.Today.AddMonths(-12), DateTime.Today);
    }
}
