using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLib.Database.Interfaces;
using eStationCore.Model.Sale.Enums;

namespace eStationCore.Model.Sale.Entity
{
    public class Purchase : Tracable
    {
        [Key]
        public Guid PurchaseGuid { get; set; }

        public Guid CompanyGuid { get; set; }

        public ProductType ProductType { get; set; }

        public Guid ProductGuid { get; set; }

        public double Quantity { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public PurchaseState PurchaseState { get; set; }

        public double Sum { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }



        [ForeignKey("CompanyGuid")]
        public virtual Company Company { get; set; }

    }
}
