using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database.Interfaces;

namespace EStationCore.Model.Oil.Entity
{
    public class OilDelivery : Tracable
    {

        [Key]
        public Guid OilDeliveryGuid { get; set; }

        public Guid OilGuid { get; set; }


        public string Supplier { get; set; }

        public int QuantityDelivered { get; set; }

        public double Cost { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Description { get; set; }



        [ForeignKey("OilGuid")]
        public virtual Oil Oil { get; set; }     

    }
}
