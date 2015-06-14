using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EStation.Views.Journals
{
    internal partial class Prelev 
    {
        public Prelev()
        {
            InitializeComponent();
        }


        public void Refresh(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate) => new Task(() => Dispatcher.BeginInvoke(new Action(() 
            => _PRELEVS.ItemsSource = App.Store.Pompes.GetPrelevCards(fuelsGuids, fromDate, toDate)))).Start();
    }
}
