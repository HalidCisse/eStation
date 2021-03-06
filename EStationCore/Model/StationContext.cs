﻿using System.Data.Entity;
using eStationCore.Model.Comm.Entity;
using eStationCore.Model.Customers.Entity;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Model.Hr.Entity;
using eStationCore.Model.Oil.Entity;
using eStationCore.Model.Sale.Entity;

namespace eStationCore.Model
{
    public class StationContext : DbContext
    {

        public StationContext() : base("name=DefaultConnection")
        {
            Database.CreateIfNotExists();
            //if (Database.Exists()) return;
            //Database.Delete();
            //Database.Create();
        }




        #region SHARED

        /// <summary>
        /// Les Bons
        /// </summary>
        public virtual DbSet<Purchase> Purchases { get; set; }


        /// <summary>
        /// Les Entreprises
        /// </summary>
        public virtual DbSet<Company> Companies { get; set; }


        /// <summary>
        /// Les Clients
        /// </summary>
        public virtual DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Les Personnels
        /// </summary>
        public virtual DbSet<Staff> Staffs { get; set; }

        /// <summary>
        /// Message Privé, Email, Multicast ou Annonce
        /// </summary>
        public virtual DbSet<Conversation> Conversations { get; set; }

        /// <summary>
        /// Les Pomps
        /// </summary>
        public virtual DbSet<Pompe> Pompes { get; set; }       

        /// <summary>
        /// Les Citernes
        /// </summary>
        public virtual DbSet<Citerne> Citernes { get; set; }

        /// <summary>
        /// Les Stocks
        /// </summary>
        public virtual DbSet<FuelDelivery> FuelDeliverys { get; set; }

        /// <summary>
        /// Les prelevements
        /// </summary>
        public virtual DbSet<FuelPrelevement> FuelPrelevements { get; set; }

        /// <summary>
        /// Les Carburant
        /// </summary>
        public virtual DbSet<Fuel.Entity.Fuel> Fuels { get; set; }

        /// <summary>
        /// Les Huile
        /// </summary>
        public virtual DbSet<Oil.Entity.Oil> Oils { get; set; }


        /// <summary>
        /// Les Stock Huiles
        /// </summary>
        public virtual DbSet<OilDelivery> OilDeliveries { get; set; }


        /// <summary>
        /// Les Prelevements Huiles
        /// </summary>
        public virtual DbSet<OilPrelevement> OilPrelevements { get; set; }




        #endregion




        #region ECONOMAT

        /// <summary>
        /// Employements des Staffs
        /// </summary>
        public virtual DbSet<Employment> Employments { get; set; }

        /// <summary>
        /// Renumerations des Employers
        /// </summary>
        public virtual DbSet<Salary> Salaries { get; set; }

        /// <summary>
		/// Methode des Payements des salaires des Staffs et des Enseignants
		/// </summary>		
        public virtual DbSet<Payroll> Payrolls { get; set; }


        /// <summary>
        /// Transactions Caisse
        /// </summary>
        public virtual DbSet<Transaction> Transactions { get; set; }


        #endregion

    }
}
