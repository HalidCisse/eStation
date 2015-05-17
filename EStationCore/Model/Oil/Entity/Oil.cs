using System;
using System.ComponentModel.DataAnnotations;
using EStationCore.Model.Common.Enums;

namespace EStationCore.Model.Oil.Entity
{
    public class Oil
    {

        [Key]
        public Guid OilGuid { get; set; }


        public TypeHuile TypeOil { get; set; }



        public double Seuil { get; set; }



    }
}
