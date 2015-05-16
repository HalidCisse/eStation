
using System;


namespace EStationCore.Model.Fuel.Views
{
    public class FuelCard
    {

        public FuelCard(Entity.Fuel fuel)
        {
            FuelGuid = fuel.FuelGuid;
            Libel = fuel.Libel;
            Threshold = fuel.Threshold.ToString("0.##");
        }



        public Guid FuelGuid { get; set; }


        public string Libel { get; set; }

        public string Threshold { get; set; }




    }
}
