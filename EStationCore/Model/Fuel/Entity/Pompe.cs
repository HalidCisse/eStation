using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;

namespace EStationCore.Model.Fuel.Entity
{
    public class Pompe : Tracable
    {

        [Key]
        public Guid PompeGuid { get; set; }

        public Guid? CiterneGuid { get; set; }



        public string Libel { get; set; }

        public string Colonne { get; set; }

        public string Number { get; set; }

        public double InitialCounter { get; set; }

        public int PistolNumber { get; set; }



       

        [ForeignKey("CiterneGuid")]
        public virtual Citerne Citerne { get; set; }

        //[ForeignKey("ColonneGuid")]
        //public virtual Colonne Colonne { get; set; }


        public virtual ICollection<Prelevement> Prelevements { get; set; } = new HashSet<Prelevement>();


    }
}
