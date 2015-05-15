using System;

namespace EStationCore.Model.Customers.Views
{
    public class CustomerCard
    {       
        public CustomerCard(Customers.Entity.Customer currentCustomer)
        {
            CustomerGuid = currentCustomer.CustomerGuid;
            Title = currentCustomer.Person.Title.ToString();
            FirstName = currentCustomer.Person.FirstName;
            LastName = currentCustomer.Person.LastName;
            PhotoIdentity = currentCustomer.Person.PhotoIdentity;
            PhoneNumber = currentCustomer.Person.PhoneNumber;
            EmailAdress = currentCustomer.Person.EmailAdress;
            HomeAdress = currentCustomer.Person.HomeAdress;
            PersonGuid = currentCustomer.Person.PersonGuid;
            Company = currentCustomer.Company;
            Status = currentCustomer.CustomerStatus.ToString();


            //using (var db = new StationContext())
            //{
            //    var currentInscription = db.Enrollements.Where(e => e.StudentGuid == currentCustomer.StudentGuid).OrderByDescending(e => e.DateAdded).FirstOrDefault();

            //    if (currentInscription != null)
            //    {

            //        CurrentClasseLevel = currentInscription.Classe.Sigle;
            //        CurrentFiliere = currentInscription.Classe.Filiere.Sigle;
            //    }
            //    else
            //    {
            //        CurrentClasseLevel = "Non Inscrit";
            //        CurrentFiliere = "Non Inscrit";
            //    }
            //}
        }



        
        public Guid CustomerGuid { get; }

        
        public Guid PersonGuid { get; }

        
        public string Title { get; }

        
        public string FirstName { get; }

        
        public string LastName { get; }

       
        public byte[] PhotoIdentity { get; }

        
        public string PhoneNumber { get; }

        
        public string EmailAdress { get; set; }

       
        public string HomeAdress { get; }

        
        public string FullName => FirstName + " " + LastName;

        
        public string Company { get; }

        
        public string Status { get; }


    }
}
