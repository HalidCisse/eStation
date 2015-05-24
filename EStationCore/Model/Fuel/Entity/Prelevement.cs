using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;


namespace EStationCore.Model.Fuel.Entity
{
    public class Prelevement : Tracable
    {
        [Key]
        public Guid PrelevementGuid { get; set; }

        public Guid PompeGuid { get; set; }

        public Guid CiterneGuid { get; set; }



        public double Meter { get; set; }

        public double MeterE { get; set; }

        public double ActualPrice { get; set; }

        public DateTime? DatePrelevement { get; set; }
            


        [ForeignKey("PompeGuid")]
        public virtual Pompe Pompe { get; set; }

        //[ForeignKey("CiterneGuid")]
        //public virtual Citerne Citerne { get; set; }

    }
}
