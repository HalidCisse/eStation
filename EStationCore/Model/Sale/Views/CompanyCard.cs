using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EStationCore.Model.Common.Enums;
using EStationCore.Model.Sale.Entity;

namespace EStationCore.Model.Sale.Views
{
    public class CompanyCard 
    {
        public CompanyCard(Company company)
        {
            CompanyGuid = company.CompanyGuid;
            PhoneNumber = $"Tel : {company.Address.PhoneNumber}";
            Name = company.Name;
            CustomerStatus = company.CustomerStatus;
            Comment = company.Comment;
            FullAddress = $"{company.Address.Line1}, BP: {company.Address.PostCode}";
        }


        public Guid CompanyGuid { get;}

        public string PhoneNumber { get; }

        public string Name { get; }

        public CustomerStatus CustomerStatus { get; }

        public string Comment { get; }

        public string FullAddress { get; }

    }
}
