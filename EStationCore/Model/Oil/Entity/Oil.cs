﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eLib.Database.Interfaces;

namespace eStationCore.Model.Oil.Entity
{
    public class Oil : Tracable
    {

        [Key]
        public Guid OilGuid { get; set; }

        public string Libel { get; set; }

        public double LiterPerGallon { get; set; }

        public double CurrentUnitPrice { get; set; }

        public DateTime? LastPriceUpdate { get; set; }

        public int StockCapacity { get; set; }

        public int InitialStock { get; set; }

        public int Threshold { get; set; }

        public string TypeOil { get; set; }
   
        public string Description { get; set; }




        public virtual ICollection<OilDelivery> Deliveries { get; set; } = new HashSet<OilDelivery>();

        public virtual ICollection<OilPrelevement> Prelevements { get; set; } = new HashSet<OilPrelevement>();

    }
}
