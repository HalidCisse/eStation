using System;
using System.ComponentModel.DataAnnotations;

namespace EStationCore.Model.Fuel.Entity
{
    internal class Carburant
    {
        [Key]
        public Guid CarburantGuid { get; set; }


        public string Nom { get; set; }


        public string TypeCarburant { get; set; }


        public double Seuil { get; set; }




    }
}
