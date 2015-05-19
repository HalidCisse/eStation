using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;

namespace EStationCore.Model.Fuel.Entity
{
    public class FuelStock : Tracable
    {
        [Key]
        public Guid FuelStockGuid { get; set; }       

        public Guid CiterneGuid { get; set; }

        

        public string Fournisseur { get; set; }

        public double Quantite { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }




        [ForeignKey("CiterneGuid")]
        public virtual Citerne Citerne { get; set; }

        //[ForeignKey("FuelGuid")]
        //public virtual Fuel Fuel { get; set; }

        //[ForeignKey("FournisseurGuid")]
        //public virtual Fournisseur Fournisseur { get; set; }

    }
}
