using System;
using System.Linq;
using eStationCore.Model.Fuel.Entity;
using Humanizer;

namespace eStationCore.Model.Fuel.Views
{
    public class PompeCard
    {
        public PompeCard(Pompe pompe)
        {
            PompeGuid = pompe.PompeGuid;
            Libel = pompe.Libel;
            Fuel = pompe.Citerne.Fuel.Libel;

            var prelev = pompe.Prelevements.OrderByDescending(p => p.DatePrelevement).FirstOrDefault();

            LastPrelevement = prelev?.DatePrelevement.GetValueOrDefault().Humanize(false);

            Meter= prelev?.Meter.ToString("0.##");
            Result = prelev?.Result.ToString("0.## 'Litres'");
        }

        
        public Guid PompeGuid { get; }
        
        public string Libel { get; }

        public string Fuel { get; }

        public string LastPrelevement { get; }

        public string Result { get; }

        public string Meter { get; }
        
    }
}
