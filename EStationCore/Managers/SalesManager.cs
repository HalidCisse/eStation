using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using CLib.Database.Entity;
using EStationCore.Model;
using EStationCore.Model.Sale.Entity;
using EStationCore.Model.Sale.Enums;
using EStationCore.Model.Sale.Views;
using Humanizer;

namespace EStationCore.Managers
{
    public class SalesManager 
    {


        #region CRUD

        public async Task<bool> Post(Company newCompany)
        {           
            using (var db = new StationContext())
            {
                if (newCompany.CompanyGuid == Guid.Empty) newCompany.CompanyGuid = Guid.NewGuid();
                if (newCompany.AddressGuid == Guid.Empty) newCompany.AddressGuid = Guid.NewGuid();
                newCompany.Address.AddressGuid = newCompany.AddressGuid;                

                newCompany.DateAdded = DateTime.Now;
                newCompany.LastEditDate = DateTime.Now;

                db.Companies.Add(newCompany);
                db.Set<Address>().Add(newCompany.Address);
                return await db.SaveChangesAsync() > 0;
            }            
        }

        public async Task<bool> Put(Company myCompany)
        {
            bool result;
            using (var db = new StationContext())
            {
                myCompany.LastEditDate = DateTime.Now;

                db.Companies.Attach(myCompany);
                db.Entry(myCompany).State = EntityState.Modified;
                result = await db.SaveChangesAsync() > 0;
            }
            return result;
        }

        public bool Delete(Guid companyGuid)
        {
            using (var db = new StationContext())
            {
                db.Companies.Remove(db.Companies.Find(companyGuid));
                return db.SaveChanges() > 0;
            }
        }

        public async Task<Company> Get(Guid companyGuid)
        {
            using (var db = new StationContext())
                return await db.Companies.FindAsync(companyGuid);
        }



        public async Task<bool> Post(Purchase myPurchase)
        {
            using (var db = new StationContext())
            {                
                switch (myPurchase.ProductType)
                {
                    case ProductType.Fuel:
                        myPurchase.Sum = myPurchase.Quantity * (await FuelManager.GetFuelCurrentPrice(myPurchase.ProductGuid));
                        myPurchase.Description = $"{myPurchase.Quantity.ToString("0.##\\ L")} {(await db.Fuels.FindAsync(myPurchase.ProductGuid)).Libel}";
                        break;
                    case ProductType.Oil:
                        myPurchase.Sum = myPurchase.Quantity * (await OilManager.StaticGet(myPurchase.ProductGuid)).CurrentUnitPrice;
                        myPurchase.Description = $"{"Bidon".ToQuantity((int) myPurchase.Quantity)} {(await db.Oils.FindAsync(myPurchase.ProductGuid)).Libel}";
                        break;                    
                }
                              
                if (myPurchase.PurchaseGuid == Guid.Empty) myPurchase.PurchaseGuid = Guid.NewGuid();
                if (myPurchase.PurchaseState == PurchaseState.Paid) myPurchase.PurchaseDate = DateTime.Now;
                myPurchase.DateAdded = myPurchase.PurchaseDate;
                myPurchase.LastEditDate = DateTime.Now;

                db.Purchases.Add(myPurchase);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public bool Put(Purchase myPurchase)
        {
            using (var db = new StationContext())
            {
                myPurchase.LastEditDate = DateTime.Now;

                db.Purchases.Attach(myPurchase);
                db.Entry(myPurchase).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool DeletePurchase(Guid purchaseGuid)
        {
            using (var db = new StationContext())
            {
                db.Purchases.Remove(db.Purchases.Find(purchaseGuid));
                return db.SaveChanges() > 0;
            }
        }

        public async Task<Purchase> GetPurchase(Guid purchaseGuid)
        {
            using (var db = new StationContext())
                return await db.Purchases.FindAsync(purchaseGuid);
        }

        public async Task<bool> CheckOut(Guid purchaseGuid, PurchaseState purchaseState = PurchaseState.Paid)
        {
            using (var db = new StationContext())
            {
                var myPurchase = await db.Purchases.FindAsync(purchaseGuid);
                if (myPurchase == null)
                    throw new InvalidOperationException("ITEM_NOT_FOUND");
                if (myPurchase.PurchaseState == purchaseState)
                    throw new InvalidOperationException("ITEM_ALREADY_PAID");

                myPurchase.PurchaseState = purchaseState;
                db.Purchases.Attach(myPurchase);
                db.Entry(myPurchase).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        #endregion



        #region Views

        public async Task<double> GetPurchasedSum(ProductType? product, PurchaseState purchaseState, DateTime? startDate, DateTime? endDate) 
            => await StaticGetPurchasedSum(product, purchaseState, startDate, endDate);


        public async Task<List<PurchaseCard>> GetPurchasesCards(List<Guid> companiesGuids, DateTime fromDate, DateTime toDate)
        {
           return await Task.Run(() => {                
                using (var db = new StationContext())
                {
                        var purchases = new List<Purchase>();
                        foreach (var companyPurchases in db.Companies.Where(f => companiesGuids.Contains(f.CompanyGuid)).Select(d => d.Purchases))
                        purchases.AddRange(companyPurchases.Where(p => p.PurchaseDate.GetValueOrDefault().Date >= fromDate && p.PurchaseDate.GetValueOrDefault().Date <= toDate));
                    return purchases.OrderByDescending(p => p.PurchaseDate).Select(p => new PurchaseCard(p)).ToList();
                } });           
        }


        public async Task<List<CompanyCard>> GetCompaniesCards()
        {
           return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Companies.ToList().Select(c=> new CompanyCard(c)).ToList();
            });            
        }


        #endregion




        #region Analytics

        
        public async Task<List<KeyValuePair<DateTime, double>>> MonthlyPurchasedSum(ProductType? product, PurchaseState purchaseState, DateTime fromDate, DateTime toDate)
        {
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await StaticGetPurchasedSum(product, purchaseState, date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;
        }

        #endregion




        #region Protected Internal Static



        internal static async Task<double> StaticGetPurchasedSum(ProductType? product, PurchaseState purchaseState, DateTime? startDate, DateTime? endDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()) {
                    switch (product)
                    {
                        case null:
                            if (!db.Purchases.Any(t => t.PurchaseState == purchaseState && !t.IsDeleted))
                                return 0;

                            if (startDate == null || endDate == null)
                                return db.Purchases.Where(t => t.PurchaseState == purchaseState && !t.IsDeleted).Sum(t => t.Sum);

                            return db.Purchases.Any(t =>
                                t.PurchaseState == purchaseState && !t.IsDeleted &&
                                t.PurchaseDate >= startDate &&
                                t.PurchaseDate <= endDate)
                                ? db.Purchases.Where(t =>
                                    t.PurchaseState == purchaseState && !t.IsDeleted &&
                                    t.PurchaseDate >= startDate &&
                                    t.PurchaseDate <= endDate
                                    ).Sum(t => t.Sum)
                                : 0;
                    }

                    if (!db.Purchases.Any(t => t.ProductType == product && t.PurchaseState == purchaseState && !t.IsDeleted))
                        return 0;

                    if (startDate == null || endDate == null)
                        return db.Purchases.Where(t => t.ProductType == product && t.PurchaseState == purchaseState && !t.IsDeleted).Sum(t => t.Sum);

                    return db.Purchases.Any(t =>
                        t.ProductType == product && t.PurchaseState == purchaseState && !t.IsDeleted &&
                        t.PurchaseDate >= startDate &&
                        t.PurchaseDate <= endDate)
                        ? db.Purchases.Where(t =>
                            t.ProductType == product && t.PurchaseState == purchaseState && !t.IsDeleted  &&
                            t.PurchaseDate >= startDate &&
                            t.PurchaseDate <= endDate
                            ).Sum(t => t.Sum)
                        : 0;
                }
            });
        }
        
        #endregion

        
    }
}
