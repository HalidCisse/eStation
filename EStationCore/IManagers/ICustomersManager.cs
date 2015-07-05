using System;
using System.Collections;
using System.Collections.Generic;
using eStationCore.Model.Common.Entity;
using eStationCore.Model.Customers.Entity;
using eStationCore.Model.Customers.Views;
using eStationCore.Model.Hr.Entity;

namespace eStationCore.IManagers
{
    public interface ICustomersManager
    {
        bool AddCustomer(Customer newCustomer);
        IEnumerable AllBirthPlaces();
        IEnumerable AllNationalities();
        bool CustomerEmailExist(string staffEMail);
        bool CustomerIdExist(string staffId);
        bool Delete(Guid customerGuid);
        IEnumerable GetAllCustomers();
        Customer GetCustomer(string customerMatricule);
        Customer GetCustomer(Guid customerGuid);
        Staff GetCustomerByEmail(string email);
        IEnumerable<CustomerCard> GetCustomersCards();
        Person GetPerson(Guid personGuid);
        Staff GetStaffByGuid(Guid staffGuid);
        string NewMatricule();
        IEnumerable<CustomerCard> Search(string searchString, int maxResult = 7);
        bool UpdateCustomer(Customer myCustomer);
        IEnumerable<KeyValuePair<string, int>> YearlyCustomers(int numberOfYear = 10);
    }
}