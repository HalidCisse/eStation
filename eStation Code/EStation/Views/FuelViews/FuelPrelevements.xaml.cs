using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStation.Views.FuelViews
{
    

    internal partial class FuelPrelevements 
    {
      
        public FuelPrelevements()
        {
            InitializeComponent();
        }


        public async Task Refresh(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            _BUSY_INDICATOR.IsBusy = true;
            await Dispatcher.BeginInvoke(new Action(()
                => _PRELEVS.ItemsSource = App.Store.Pompes.GetPrelevCards(fuelsGuids, fromDate, toDate)));
            _BUSY_INDICATOR.IsBusy = false;
        }
    }
}
