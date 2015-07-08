using System;
using System.Collections;
using System.Collections.Generic;
using eStationCore.Model.Common.Entity;
using eStationCore.Model.Common.Views;
using eStationCore.Model.Hr.Entity;
using eStationCore.Model.Hr.Views;

namespace eStationCore.IManagers
{
    public interface IHrManager
    {
        bool AddStaff(Staff myStaff);
        IEnumerable AllBirthPlaces();
        IEnumerable AllCategories();
        IEnumerable AllDepartement();
        IEnumerable AllDepartements();
        IEnumerable<string> AllDivisions();
        IEnumerable AllGrades();
        IEnumerable AllNationalities();
        IEnumerable AllPositions();
        IEnumerable AllProjets();
        IEnumerable AllStaffPositions();
        IEnumerable AllStaffQualifications();
        IEnumerable AllStaffsNames();
        bool DeleteStaff(Guid staffGuid);
        IEnumerable GetAllStaffs();
        Dictionary<string, Guid> GetAllStaffsDictionary();
        List<Staff> GetDepStaffs(string depName = null);
        List<DepStaffCard> GetDepStaffsCard();
        string GetNewStaffMatricule();
        Person GetPerson(Guid personGuid);
        Staff GetStaff(string staffMatricule);
        Staff GetStaff(Guid staffGuid);
        Staff GetStaffByEmail(string email);
        string GetStaffFullName(string staffId);
        HashSet<SearchCard> Search(string searchString, int maxResult = 7);
        bool StaffEmailExist(string staffEMail);
        bool StaffIdExist(string staffId);
        IEnumerable<KeyValuePair<string, int>> StaffPerYear(int numberOfYear = 10);
        bool UpdateStaff(Staff mStaff);
    }
}