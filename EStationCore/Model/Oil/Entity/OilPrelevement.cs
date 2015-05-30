using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;

namespace EStationCore.Model.Oil.Entity
{
    public class OilPrelevement :Tracable
    {
        [Key]
        public Guid OilPrelevementGuid { get; set; }

        public Guid OilGuid { get; set; }
      


        public double TotalMeter { get; set; }

        public double DiffMeter { get; set; }

        public double ActualUnitPrice { get; set; }

        public DateTime? DatePrelevement { get; set; }



        [ForeignKey("OilGuid")]
        public virtual Oil Oil { get; set; }

    }
}
