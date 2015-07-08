using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eStationCore.Model.Fuel.Entity;

namespace eStationCore.Model.Sale.Entity
{
    public class Journal
    {
        [Key]
        public Guid JournalGuid { get; set; }





        public List<FuelPrelevement> Prelevements { get; set; }

    }
}
