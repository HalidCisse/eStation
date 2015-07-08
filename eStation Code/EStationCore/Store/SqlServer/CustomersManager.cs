using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using Bytes2you.Validation;
using CLib;
using eStationCore.IManagers;
using eStationCore.Model;
using eStationCore.Model.Common.Entity;
using eStationCore.Model.Customers.Entity;
using eStationCore.Model.Customers.Views;
using eStationCore.Model.Hr.Entity;

namespace eStationCore.Store.SqlServer
{
    public sealed class CustomersManager : ICustomersManager
    {
        private readonly StationContext Db;

        public CustomersManager(StationContext stationContext)
        {
            Db = stationContext;
        }

        #region CRUD

        /// <summary>
        /// Represente un client
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <exception cref="InvalidOperationException">CAN_NOT_CREAT_STAFF_PROFILE</exception>
        /// <returns></returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.CustomerWrite)]
        public bool AddCustomer(Customer newCustomer) 
        {
            Guard.WhenArgument(newCustomer.Person.FullName, "CUSTOMER_NAME_CAN_NOT_BE_EMPTY").IsNullOrEmpty().IsEqual("Inconnue").Throw();

            using (var db = new StationContext())
            {
                if (newCustomer.CustomerGuid == Guid.Empty)
                    newCustomer.CustomerGuid = Guid.NewGuid();
                if (newCustomer.Person.PersonGuid == Guid.Empty)
                    newCustomer.Person.PersonGuid = Guid.NewGuid();

                // ReSharper disable once PossibleNullReferenceException
                var userTrace = (Guid) Membership.GetUser().ProviderUserKey;
                newCustomer.DateAdded = DateTime.Now;
                newCustomer.AddUserGuid = userTrace;
                newCustomer.LastEditDate = DateTime.Now;
                newCustomer.LastEditUserGuid = userTrace;

                db.Set<Person>().Add(newCustomer.Person);
                db.Customers.Add(newCustomer);
                return db.SaveChanges() > 0;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="myCustomer"></param>
        /// <returns></returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.StaffWrite)]
        public bool UpdateCustomer(Customer myCustomer)
        {
            using (var db = new StationContext())
            {
                // ReSharper disable once PossibleNullReferenceException
                var userTrace = (Guid)Membership.GetUser().ProviderUserKey;                
                myCustomer.LastEditDate = DateTime.Now;
                myCustomer.LastEditUserGuid = userTrace;

                db.Customers.Attach(myCustomer);
                db.Entry(myCustomer).State = EntityState.Modified;

                db.Set<Person>().Attach(myCustomer.Person);
                db.Entry(myCustomer.Person).State = EntityState.Modified;

                return db.SaveChanges() > 0;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerGuid"></param>
        /// <returns></returns>
        //[PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.StaffDelete)]
        public bool Delete(Guid customerGuid)
        {
            using (var db = new StationContext())
            {
                var theMan = db.Customers.Find(customerGuid);

                Guard.WhenArgument(theMan, "CAN_NOT_FIND_STAFF_REFERENCE").IsNull().Throw();

                theMan.Person.DeleteDate = DateTime.Now;
                theMan.Person.IsDeleted = true;
                // ReSharper disable once PossibleNullReferenceException
                theMan.Person.DeleteUserGuid = (Guid)Membership.GetUser().ProviderUserKey;

                db.Customers.Attach(theMan);
                db.Entry(theMan).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        #endregion



        #region HELPERS


        public IEnumerable<CustomerCard> GetCustomersCards()
        {
            using (var db = new StationContext())
                return db.Customers.Where(s => !s.Person.IsDeleted)
                    .OrderBy(s => s.Person.FirstName)
                    .Include(s => s.Person)
                    .ToList()
                    .Select(student => new CustomerCard(student));
        }


        /// <summary>
        /// Renvoi la list des Staff 
        /// </summary>
        /// <param name="searchString">Parametre de Recherche</param>
        /// <param name="maxResult">Nombre max de Resultat</param>
        /// <returns></returns>        
        public IEnumerable<CustomerCard> Search(string searchString, int maxResult = 7)
        {
            searchString = searchString?.Trim();

            if (!string.IsNullOrEmpty(searchString))
                using (var db = new StationContext())
                {
                    return db.Customers.Where(s => (s.Person.FirstName + " " + s.Person.LastName).Contains(searchString) ||
                                                        (s.Person.LastName + " " + s.Person.FirstName).Contains(searchString) ||
                                                         s.Person.EmailAdress.Contains(searchString) ||
                                                         s.Matricule.Equals(searchString)
                                     ).Take(7)
                                     .Include(s => s.Person)
                                     .ToList()
                                     .Select(s => new CustomerCard(s));
                }
            using (var db = new StationContext())
                return db.Customers.Include((s => s.Person))
                                  .ToList()
                                  .Select(s => new CustomerCard(s));           
        }

       
        public Staff GetStaffByGuid(Guid staffGuid)
        {
            using (var db = new StationContext())
                return QueryableExtensions.Include(db.Staffs, s => s.Person).FirstOrDefault(s => s.StaffGuid == staffGuid);
        }

        
        public IEnumerable GetAllCustomers()
        {
            using (var db = new StationContext())
                return db.Customers.Include( (s => s.Person)).OrderBy(s => s.Person.FirstName).ToList();
        }

       
        public Person GetPerson(Guid personGuid)
        {
            using (var db = new StationContext())
                return db.Set<Person>().Find(personGuid);
        }

       
        public Staff GetCustomerByEmail(string email)
        {
            using (var db = new StationContext())
                return QueryableExtensions.Include(db.Staffs, s => s.Person).FirstOrDefault(s => s.Person.EmailAdress.Equals(email));
        }

        
        public Customer GetCustomer(string customerMatricule)
        {
            using (var db = new StationContext())
                return db.Customers.Include((s => s.Person)).FirstOrDefault(s => s.Matricule.Equals(customerMatricule));
        }

     
        public Customer GetCustomer(Guid customerGuid)
        {
            using (var db = new StationContext())
                return db.Customers.Include( (s => s.Person)).FirstOrDefault(s => s.CustomerGuid == customerGuid);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public bool CustomerIdExist(string staffId)
        {
            using (var db = new StationContext())
            {
                return db.Staffs.Any(s => s.Matricule == staffId);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffEMail"></param>
        /// <returns></returns>
        ///[PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public bool CustomerEmailExist(string staffEMail)
        {
            using (var db = new StationContext())
            {
                return db.Staffs.Any(s => s.Person.EmailAdress.Equals(staffEMail));
            }
        }

        
        public string NewMatricule()
        {
            string newStaffId;

            do newStaffId = "S" + RandomHelper.GetRandLetters(1) + "-" + DateTime.Today.Month + DateTime.Today.Year.ToString().Substring(2) + "-" + RandomHelper.GetRandNum(4);
            while (
                     CustomerIdExist(newStaffId)
                  );

            return newStaffId;
        }

                    
        /// <summary>
        /// Return les nationalites des etudiants ainsi que des staffs
        /// </summary>
        /// <returns></returns>
        public IEnumerable AllNationalities()
        {
            var nationalities = new List<string>();

            using (var db = new StationContext())
                nationalities.AddRange(db.Set<Person>().Select(p => p.Nationality).ToList());

            if (nationalities.Count != 0) return nationalities.Distinct();

            nationalities.Add("Maroc");
            nationalities.Add("Mali");
            nationalities.Add("US");
            nationalities.Add("France");
            nationalities.Add("Senegal");
            nationalities.Add("Algerie");
            nationalities.Add("Liberia");
            nationalities.Add("Guinee");
            nationalities.Add("Afrique Du Sud");
            nationalities.Add("Nigeria");
            nationalities.Add("Soudan");
            nationalities.Add("Gambie");
            nationalities.Add("Congo");
            nationalities.Add("Burkina Fasso");

            return nationalities.Distinct();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable AllBirthPlaces()
        {
            var birthPlace = new List<string>();

            using (var db = new StationContext())
            {
                var studentBp = (from s in db.Customers.ToList() where s.Person.BirthPlace != null select s.Person.BirthPlace).ToList().Distinct().ToList();
                birthPlace.AddRange(studentBp);

                var staffBp = (from s in db.Staffs.ToList() where s.Person.BirthPlace != null select s.Person.BirthPlace).ToList().Distinct().ToList();
                birthPlace.AddRange(staffBp);
            }

            if (birthPlace.Count != 0) return birthPlace.Distinct();

            birthPlace.Add("Rabat");
            birthPlace.Add("Casablanca");
            birthPlace.Add("Bamako");
            birthPlace.Add("Toumbouctou");
            birthPlace.Add("Tayba");
            birthPlace.Add("Dakar");

            return birthPlace;
        }



        #endregion



        #region Analytic




        public IEnumerable<KeyValuePair<string, int>> YearlyCustomers(int numberOfYear = 10)
        {
            using (var db = new StationContext())
            {
                foreach (var year in DateTimeHelper.EachYear(DateTime.Today.AddYears(-numberOfYear), DateTime.Today))
                    yield return
                        new KeyValuePair<string, int>(year.Year.ToString(),
                            db.Customers.Count(s => s.Person.DateAdded.Value.Year == year.Year));
            }
        }

        #endregion



        #region internal Static


        internal static Staff StaticGetStaffByGuid(Guid staffGuid)
        {
            try
            {
                using (var db = new StationContext())
                {
                    var x = QueryableExtensions.Include(db.Staffs, s => s.Person).FirstOrDefault(s => s.StaffGuid == staffGuid);
                    return x;
                }
            }
            catch (Exception exception)
            {
                DebugHelper.WriteException(exception);
                //throw;
            }
            return null;
        }



        #endregion



    }
}
