using System;
using EStationCore.Model.Fuel.Entity;



namespace EStationCore.Model.Fuel.Views
{
    public class CiterneCard
    {

        public CiterneCard(Citerne citerne)
        {
            CiterneGuid = citerne.CiterneGuid;
            Libel = citerne.Libel;
        }


        public Guid CiterneGuid { get; set; }

        public string Libel { get; set; }

        



    }
}
