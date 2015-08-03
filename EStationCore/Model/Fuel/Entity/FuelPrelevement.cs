using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLib.Database.Interfaces;

namespace eStationCore.Model.Fuel.Entity
{
    public class FuelPrelevement : Tracable
    {
        [Key]
        public Guid PrelevementGuid { get; set; }

        public Guid PompeGuid { get; set; }

        public Guid CiterneGuid { get; set; }



        public double Meter { get; set; }

        public double Result { get; set; }

        public double CurrentPrice { get; set; }

        public DateTime? DatePrelevement { get; set; }
            


        [ForeignKey("PompeGuid")]
        public virtual Pompe Pompe { get; set; }

        //[ForeignKey("CiterneGuid")]
        //public virtual Citerne Citerne { get; set; }

    }
}
