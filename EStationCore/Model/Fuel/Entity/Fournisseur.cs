using System;
using System.ComponentModel.DataAnnotations;

namespace EStationCore.Model.Fuel.Entity
{
    public class Fournisseur
    {
        [Key]
        public Guid FournisseurGuid { get; set; }

        public string Nom { get; set; }


        public string Tel { get; set; }


        public string Fax { get; set; }


        public string Adresse { get; set; }





    }
}
