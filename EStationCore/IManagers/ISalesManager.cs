using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStationCore.Model.Sale.Entity;
using eStationCore.Model.Sale.Enums;
using eStationCore.Model.Sale.Views;

namespace eStationCore.IManagers
{
    public interface ISalesManager
    {
        Task<bool> CheckOut(Guid purchaseGuid, PurchaseState purchaseState = PurchaseState.Paid);
        Task<bool> Delete(Guid companyGuid);
        Task<bool> DeletePurchase(Guid purchaseGuid);
        Task<Company> Get(Guid companyGuid);
        Task<List<CompanyCard>> GetCompaniesCards();
        Task<Purchase> GetPurchase(Guid purchaseGuid);
        Task<double> GetPurchasedSum(ProductType? product, PurchaseState purchaseState, DateTime? startDate, DateTime? endDate);
        Task<List<PurchaseCard>> GetPurchasesCards(List<Guid> companiesGuids, DateTime fromDate, DateTime toDate);
        Task<List<KeyValuePair<DateTime, double>>> MonthlyPurchasedSum(ProductType? product, PurchaseState purchaseState, DateTime fromDate, DateTime toDate);
        Task<bool> Post(Purchase myPurchase);
        Task<bool> Post(Company newCompany);
        bool Put(Purchase myPurchase);
        Task<bool> Put(Company myCompany);
    }
}