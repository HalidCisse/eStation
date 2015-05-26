using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;

namespace EStationCore.Model.Fuel.Entity
{
    public class FuelDelivery : Tracable
    {
        [Key]
        public Guid FuelDeliveryGuid { get; set; }       

        public Guid CiterneGuid { get; set; }

        

        public string Supplier { get; set; }

        public double QuantityDelivered { get; set; }

        public double Cost { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Description { get; set; }



        [ForeignKey("CiterneGuid")]
        public virtual Citerne Citerne { get; set; }

        //[ForeignKey("FuelGuid")]
        //public virtual Fuel Fuel { get; set; }

        //[ForeignKey("FournisseurGuid")]
        //public virtual Fournisseur Fournisseur { get; set; }

    }
}
