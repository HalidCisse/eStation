using System;
using EStationCore.Managers;
using Humanizer;



namespace EStationCore.Model.Oil.Views
{
    public class OilCard
    {
        public OilCard(Entity.Oil oil)
        {
            OilGuid = oil.OilGuid;
            Libel = oil.Libel;
            TypeOil = oil.TypeOil;
            var curBalance = OilManager.StaticGetOilBalance(oil.OilGuid);
            if (oil.StockCapacity > 0){
                CurrentStock = "bidon".ToQuantity(curBalance) + " en Stock/ " + "bidon".ToQuantity(oil.StockCapacity);
                Pourcentage = ((curBalance*100)/oil.StockCapacity).ToString("0.##\\%");
            }
            else
                CurrentStock = "bidon".ToQuantity(curBalance) + " en Stock";
           
            CurrentUnitPrice = oil.CurrentUnitPrice.ToString("0.##\\dhs/L");
        }


        public Guid OilGuid { get; }

        public string Libel { get; }

        public string TypeOil { get; }

        public string CurrentStock { get; }

        public string Pourcentage { get; }

        public string CurrentUnitPrice { get; }

    }
}
