using System;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Store.SqlServer;

namespace eStationCore.Model.Fuel.Views
{
    public class CiterneCard
    {

        public CiterneCard(Citerne citerne)
        {
            CiterneGuid = citerne.CiterneGuid;
            Libel = citerne.Libel;
            Fuel = citerne.Fuel.Libel;
            var curBalance = CiternesManager.StaticGetCiterneFuelBalance(citerne.CiterneGuid).Result;
            CurrentStock = curBalance.ToString("0.##\\L") + " en stock /" + citerne.MaxCapacity.ToString("0.##\\L") ;
            Pourcentage = ((curBalance * 100)/citerne.MaxCapacity).ToString("0.##\\%");
            //FuelPrice = FuelManager.GetFuelCurrentPrice(citerne.Fuel.FuelGuid).ToString("0.##\\dhs/L");
        }


        public Guid CiterneGuid { get;  }

        public string Libel { get;  }

        public string Fuel { get;  }

        public string CurrentStock { get; }

        public string Pourcentage { get; }

        //public string FuelPrice { get; }

    }
}
