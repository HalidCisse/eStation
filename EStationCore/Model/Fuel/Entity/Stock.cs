using System;
using System.ComponentModel.DataAnnotations;
using EStationCore.Model.Common.Enums;

namespace EStationCore.Model.Fuel.Entity
{
    internal class Stock
    {
        [Key]
        public Guid StockGuid { get; set; }

        public StockType StockType { get; set; }


        public double Quantite { get; set; }




        public Fournisseur Fournisseur { get; set; }
    }
}
