using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CLib.Database.Interfaces;

namespace EStationCore.Model.Fuel.Entity
{
    public class Fuel : Tracable
    {
        [Key]
        public Guid FuelGuid { get; set; }


        public string Libel { get; set; }

        public double ActualPrice { get; set; }

        public string TypeFuel { get; set; }

        public double Threshold { get; set; }
       
        public string Description { get; set; }



        public virtual ICollection<Citerne> Citernes { get; set; } = new HashSet<Citerne>();

        public virtual ICollection<Price> Prices { get; set; } = new HashSet<Price>();

    }
}
