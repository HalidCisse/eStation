using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EStationCore.Model.Fuel.Entity
{
    public class Colonne
    {
        [Key]
        public Guid ColonneGuid { get; set; }

        public string Libel { get; set; }



        public virtual ICollection<Pompe> Pompes { get; set; } = new HashSet<Pompe>();


    }
}
