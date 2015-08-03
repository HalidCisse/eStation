using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLib.Database.Interfaces;
using eStationCore.Model.Common.Entity;
using eStationCore.Model.Common.Enums;
using eStationCore.Model.Sale.Entity;

namespace eStationCore.Model.Customers.Entity
{

    /// <summary>
    /// un client
    /// </summary>
    public class Customer : Tracable
    {
        [Key]
        public Guid CustomerGuid { get; set; }

       
        public Guid PersonGuid { get; set; }

       
        public string Matricule { get; set; }


        public string Company { get; set; }


        public CustomerStatus CustomerStatus { get; set; }


        
        [ForeignKey("PersonGuid")]
        public virtual Person Person { get; set; }

        
        public virtual ICollection<Purchase> Achats { get; set; } = new HashSet<Purchase>();

    }
}
