using System;
using EStationCore.Model.Fuel.Entity;


namespace EStationCore.Model.Fuel.Views
{
    public class PompeCard
    {

        public PompeCard(Pompe pompe)
        {
            PompeGuid = pompe.PompeGuid;
            Libel = pompe.Libel;          
        }

        
        public Guid PompeGuid { get; }
        
        public string Libel { get; }

    }
}
