using System;
using EStationCore.Model.Oil.Entity;
using Humanizer;

namespace EStationCore.Model.Oil.Views
{
    public class OilPrelevCard
    {

        public OilPrelevCard(OilPrelevement prelevement)
        {
            OilPrelevementGuid = prelevement.OilPrelevementGuid;
            OilLibel = $"{prelevement.Oil.Libel.ToLower().Titleize()} ({prelevement.Oil.TypeOil.ToLower().Titleize()})";
            DatePrelev = prelevement.DatePrelevement.GetValueOrDefault().ToString("dd MMM yy - HH:mm");
            //Fuel = prelevement.Pompe.Citerne.Fuel.Libel + " (" + DatePrelev + ")";
            //MeterM = "M: " + prelevement.Meter.ToString("0.##\\ L ");
            //MeterE = "E: " + prelevement.MeterE.ToString("0.##\\ L ");

            //var curBalance = CiternesManager.StaticGetCiterneFuelBalance(citerne.CiterneGuid);
            //CurrentStock = curBalance.ToString("0.##\\L") + " en stock /" + citerne.MaxCapacity.ToString("0.##\\L");
            //Pourcentage = ((curBalance * 100) / citerne.MaxCapacity).ToString("0.##\\%");
        }

        public Guid OilPrelevementGuid { get; }

        public string OilLibel { get; }

        public string DatePrelev { get; }

        public string Libel { get; }

        public string MeterM { get; }

        public string MeterE { get; }


    }
}
