using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;

namespace EStationCore.Model.Fuel.Entity
{
    public class StockFuel : Tracable
    {
        [Key]
        public Guid StockGuid { get; set; }

        public Guid FournisseurGuid { get; set; }

        public Guid FuelGuid { get; set; }


        public string Libel { get; set; }

        public double Quantite { get; set; }



        [ForeignKey("FuelGuid")]
        public virtual Fuel Fuel { get; set; }

        [ForeignKey("FournisseurGuid")]
        public virtual Fournisseur Fournisseur { get; set; }

    }
}
