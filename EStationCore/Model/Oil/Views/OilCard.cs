using System;
using eStationCore.Managers;
using Humanizer;

namespace eStationCore.Model.Oil.Views
{
    public class OilCard
    {
        public OilCard(Entity.Oil oil)
        {
            OilGuid = oil.OilGuid;
            FullLibel = $"{oil.Libel} ({oil.TypeOil.ToUpper()})";
            Libel = oil.Libel;
            TypeOil = oil.TypeOil;
            LastPriceUpdate = oil.LastPriceUpdate.GetValueOrDefault().Humanize();

            var curBalance = OilManager.StaticGetOilBalance(oil.OilGuid);
            if (oil.StockCapacity > 0)
            {
                CurrentStock = "bidon".ToQuantity(curBalance) + " en Stock"; //+ "bidon".ToQuantity(oil.StockCapacity);
                Pourcentage = ((curBalance*100)/oil.StockCapacity).ToString("0.##\\%");
            }
            else
                CurrentStock = "bidon".ToQuantity(curBalance) + " en Stock";
           
            CurrentUnitPrice = oil.CurrentUnitPrice.ToString("0.##\\dhs/Bidon");
            CurrentdoubleUnitPrice = oil.CurrentUnitPrice;
        }


        public Guid OilGuid { get; }

        public string FullLibel { get; }

        public string Libel { get; }

        public string TypeOil { get; }

        public string CurrentStock { get; }

        public string Pourcentage { get; }

        public string CurrentUnitPrice { get; }

        public double CurrentdoubleUnitPrice { get; set; }

        public string LastPriceUpdate { get; } 

    }
}
