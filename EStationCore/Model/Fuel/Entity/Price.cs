using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;

namespace EStationCore.Model.Fuel.Entity
{
    public class Price : Tracable
    {
        [Key]
        public Guid PriceGuid { get; set; }

        public Guid ProductGuid { get; set; }


        public double Value { get; set; }

        public DateTime? FromDate{ get; set; }



        [ForeignKey("ProductGuid")]
        public virtual Fuel Fuel { get; set; }

    }
}
