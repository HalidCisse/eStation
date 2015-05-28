using System.Data.Entity;
using EStationCore.Model.Comm.Entity;
using EStationCore.Model.Customers.Entity;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Hr.Entity;
using EStationCore.Model.Oil.Entity;
using EStationCore.Model.Sale.Entity;

namespace EStationCore.Model
{
    public class StationContext : DbContext
    {

        public StationContext() : base("name=DefaultConnection")
        {

            //          Update-Database 
        }




        #region SHARED

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
