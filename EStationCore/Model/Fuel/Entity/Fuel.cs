using System;
using System.ComponentModel.DataAnnotations;
using CLib.Database;


namespace EStationCore.Model.Fuel.Entity
{
    public class Fuel : Tracable
    {
        [Key]
        public Guid CarburantGuid { get; set; }


        public string Libel { get; set; }

        public string TypeFuel { get; set; }

        public double Threshold { get; set; }



    }
}
