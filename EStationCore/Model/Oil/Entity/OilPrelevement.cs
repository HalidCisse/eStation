using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLib.Database.Interfaces;

namespace eStationCore.Model.Oil.Entity
{
    public class OilPrelevement :Tracable
    {
        [Key]
        public Guid OilPrelevementGuid { get; set; }

        public Guid OilGuid { get; set; }
      


        public int Meter { get; set; }

        public int Result { get; set; }

        public double CurrentPrice { get; set; }

        public DateTime? DatePrelevement { get; set; }



        [ForeignKey("OilGuid")]
        public virtual Oil Oil { get; set; }

    }
}
