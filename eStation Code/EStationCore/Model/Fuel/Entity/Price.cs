using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database.Interfaces;

namespace eStationCore.Model.Fuel.Entity
{
    public class Price : Tracable
    {
        [Key]
        public Guid PriceGuid { get; set; }

        public Guid ProductGuid { get; set; }


        public double ActualPrice { get; set; }

        public DateTime? FromDate{ get; set; }



        [ForeignKey("ProductGuid")]
        public virtual Fuel Fuel { get; set; }

    }
}
