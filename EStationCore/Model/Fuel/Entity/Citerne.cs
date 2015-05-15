using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;


namespace EStationCore.Model.Fuel.Entity
{
    public class Citerne :Tracable
    {
        [Key]
        public Guid CiterneGuid { get; set; }
        
        public Guid FuelGuid { get; set; }


        public string Libel { get; set; }

        public string Matricule { get; set; }

        public double Threshold { get; set; }

        public int MaxCapacity { get; set; }




        [ForeignKey("FuelGuid")]
        public virtual Fuel Fuel { get; set; }

        public virtual ICollection<Pompe> Pompes { get; set; } = new HashSet<Pompe>();

        public virtual ICollection<StockFuel> Stocks { get; set; } = new HashSet<StockFuel>();

        public virtual ICollection<Prelevement> Prelevements { get; set; } = new HashSet<Prelevement>();


    }
}
