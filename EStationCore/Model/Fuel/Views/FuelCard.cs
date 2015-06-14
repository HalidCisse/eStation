
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
             var lastPrice= FuelManager.GetFuelLastPrice(fuel.FuelGuid);
            CurrentDoublePrice = lastPrice.ActualPrice;
            CurrentPrice = lastPrice.ActualPrice.ToString("0.##\\ dhs/L");
            LastPriceUpdate = "Derniere modification du prix " + lastPrice.FromDate.GetValueOrDefault().ToLongDateString();
            CiternesNumber = " Citerne".ToQuantity(fuel.Citernes.Count);

            var curBalance = FuelManager.StaticGetFuelBalance(fuel.FuelGuid);
            CurrentStock = curBalance.ToString("0.##\\L") + " en stock /" + fuel.Citernes.Sum(c => c.MaxCapacity).ToString("0.##\\L");
            Pourcentage = ((curBalance * 100) / fuel.Citernes.Sum(c=> c.MaxCapacity)).ToString("0.##\\%");           
        }



        public Guid FuelGuid { get; }


        public string Libel { get; }

        public string Threshold { get;}

        public string CurrentPrice { get; }

        public double CurrentDoublePrice { get; set; }

        public string LastPriceUpdate { get; }

        public string CiternesNumber { get; }

        public string CurrentStock { get;}

        public string Pourcentage { get;}
    }
}
