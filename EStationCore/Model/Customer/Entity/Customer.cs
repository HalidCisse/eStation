using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CLib.Database;
using EStationCore.Model.Common.Entity;
using EStationCore.Model.Common.Enums;
using EStationCore.Model.Sale.Entity;

namespace EStationCore.Model.Customer.Entity
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
