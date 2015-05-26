using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EStationCore.Managers;
using EStationCore.Model.Fuel.Entity;
using Humanizer;

namespace EStationCore.Model.Fuel.Views
{
    public class PrelevCard
    {
        public PrelevCard(Prelevement prelevement)
        {
            PrelevementGuid = prelevement.PrelevementGuid;
            Pompe = $"{prelevement.Pompe.Libel.ToLower().Titleize()} ({prelevement.Pompe.Colonne.ToLower().Titleize()})";
            DatePrelev = prelevement.DatePrelevement.GetValueOrDefault().ToString("dd MMM yy - HH:mm");
            Fuel = prelevement.Pompe.Citerne.Fuel.Libel + " (" + DatePrelev + ")";
            MeterM = "M: " + prelevement.Meter.ToString("0.##\\ L ");
            MeterE = "E: " + prelevement.MeterE.ToString("0.##\\ L ");

            //var curBalance = CiternesManager.StaticGetCiterneFuelBalance(citerne.CiterneGuid);
            //CurrentStock = curBalance.ToString("0.##\\L") + " en stock /" + citerne.MaxCapacity.ToString("0.##\\L");
            //Pourcentage = ((curBalance * 100) / citerne.MaxCapacity).ToString("0.##\\%");
        }

        public Guid PrelevementGuid { get; }

        public string Pompe { get; }

        public string DatePrelev { get; }

        public string Fuel { get; }

        public string MeterM { get; }

        public string MeterE { get; }

    }
}
