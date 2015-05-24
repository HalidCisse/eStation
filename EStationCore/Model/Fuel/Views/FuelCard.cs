
using System;
using System.Linq;
using EStationCore.Managers;
using Humanizer;


namespace EStationCore.Model.Fuel.Views
{
    public class FuelCard
    {

        public FuelCard(Entity.Fuel fuel)
        {
            FuelGuid = fuel.FuelGuid;
            Libel = fuel.Libel;
            Threshold = fuel.Threshold.ToString("0.##");

            CurrentPrice = FuelManager.GetFuelCurrentPrice(fuel.FuelGuid).ToString("0.##\\ dhs/L");  //fuel.Prices.OrderByDescending(p => p.FromDate.GetValueOrDefault().Ticks).First().ActualPrice.ToString("0.##\\ dhs/L") ;

            CiternesNumber = " Citerne".ToQuantity(fuel.Citernes.Count);

            CurrentStock = FuelManager.GetFuelStock(fuel.FuelGuid).ToString("0.##\\ L en stock");
        }



        public Guid FuelGuid { get; set; }


        public string Libel { get; set; }

        public string Threshold { get; set; }

        public string CurrentPrice { get; set; }

        public string CiternesNumber { get; set; }

        public string CurrentStock { get; set; }
    }
}
