using System;
using System.Globalization;
using System.Linq;
using EStationCore.Model.Fuel.Entity;
using Humanizer;


namespace EStationCore.Model.Fuel.Views
{
    public class PompeCard
    {
        public PompeCard(Pompe pompe)
        {
            PompeGuid = pompe.PompeGuid;
            Libel = pompe.Libel;

            var prelev = pompe.Prelevements.OrderByDescending(p => p.DatePrelevement).FirstOrDefault();

            LastPrelevement = prelev?.DatePrelevement.GetValueOrDefault().Humanize(false);

            MeterM = prelev?.Meter.ToString(CultureInfo.InvariantCulture);
            MeterE = prelev?.MeterE.ToString(CultureInfo.InvariantCulture);
        }

        
        public Guid PompeGuid { get; }
        
        public string Libel { get; }

        public string LastPrelevement { get; }

        public string MeterE { get; }

        public string MeterM { get; }
        
    }
}
