using System;
using System.ComponentModel.DataAnnotations;
using EStationCore.Model.Common.Enums;

namespace EStationCore.Model.Oil.Entity
{
    public class Huile
    {

        [Key]
        public Guid HuileGuid { get; set; }


        public TypeHuile TypeHuile { get; set; }



        public double Seuil { get; set; }



    }
}
