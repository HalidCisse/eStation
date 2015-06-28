using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database.Interfaces;

namespace eStationCore.Model.Fuel.Entity
{
    public class Pompe : Tracable
    {

        [Key]
        public Guid PompeGuid { get; set; }

        public Guid? CiterneGuid { get; set; }



        public string Libel { get; set; }

        public string Colonne { get; set; }

        public string Number { get; set; }

        public double InitialMeter { get; set; }

        public int PistolNumber { get; set; }


       
        [ForeignKey("CiterneGuid")]
        public virtual Citerne Citerne { get; set; }

        //[ForeignKey("ColonneGuid")]
        //public virtual Colonne Colonne { get; set; }


        public virtual ICollection<FuelPrelevement> Prelevements { get; set; } = new HashSet<FuelPrelevement>();


    }
}
