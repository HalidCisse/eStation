using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStationCore.Model.Fuel.Entity
{
    public class Pompe
    {

        [Key]
        public Guid PompeGuid { get; set; }

        public Guid ColonneGuid { get; set; }

        public Guid CiterneGuid { get; set; }

        public int CompteurMecanique { get; set; }

        public double CompteurElectronique1 { get; set; }

        public double CompteurElectronique2 { get; set; }

        public double CompteurElectronique3 { get; set; }



        [ForeignKey("CiterneGuid")]
        public virtual Citerne Citerne { get; set; }

        [ForeignKey("ColonneGuid")]
        public virtual Colonne Colonne { get; set; }
    }
}
