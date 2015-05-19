using System;
using EStationCore.Managers;
using EStationCore.Model.Fuel.Entity;



namespace EStationCore.Model.Fuel.Views
{
    public class CiterneCard
    {

        public CiterneCard(Citerne citerne)
        {
            CiterneGuid = citerne.CiterneGuid;
            Libel = citerne.Libel;
            Fuel = citerne.Fuel.Libel;
            CurrentStock = CiternesManager.GetCiterneStock(citerne.CiterneGuid).ToString("0.##") + " L en stock /" + citerne.MaxCapacity.ToString("0.##") + "L";
            Pourcentage = ((CiternesManager.GetCiterneStock(citerne.CiterneGuid)*100)/citerne.MaxCapacity).ToString("P");
        }


        public Guid CiterneGuid { get;  }

        public string Libel { get;  }

        public string Fuel { get;  }

        public string CurrentStock { get; }

        public string Pourcentage { get; }

    }
}
