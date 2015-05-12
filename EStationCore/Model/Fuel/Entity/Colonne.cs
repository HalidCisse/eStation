using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EStationCore.Model.Fuel.Entity
{
    public class Colonne
    {
        [Key]
        public Guid ColonneGuid { get; set; }

        public string Nom { get; set; }



        public List<Pompe> Pompes { get; set; }


    }
}
