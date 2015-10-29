using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using eLib;
using eLib.Exceptions;
using eStationCore.IManagers;
using eStationCore.Model;
using eStationCore.Model.Oil.Entity;
using eStationCore.Model.Oil.Views;
using Humanizer;

namespace eStationCore.Store.SqlServer
{
    public class OilManager : IOilManager
    {
        private readonly StationContext _db;

        public OilManager(StationContext stationContext)
        {
            _db = stationContext;
        }

        #region CRUD


        public bool Post(Oil myOil)
        {
            using (var db = new StationContext())
            {
                if (myOil.OilGuid == Guid.Empty) myOil.OilGuid = Guid.NewGuid();

                myOil.DateAdded = DateTime.Now;
                myOil.LastEditDate = DateTime.Now;

                db.Oils.Add(myOil);
               return db.SaveChanges() > 0;              
            }
        }

        public bool Put(Oil myOil)
        {
            using (var db = new StationContext())
            {
                myOil.LastEditDate = DateTime.Now;

                db.Oils.Attach(myOil);
                db.Entry(myOil).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }
      
        public async Task<bool> Delete(Guid oilGuid)
        {
            using (var db = new StationContext())
            {
                var myObject = await db.Oils.FindAsync(oilGuid);

                if (myObject == null) throw new InvalidOperationException("OIL_NOT_FOUND");

                myObject.LastEditDate = DateTime.Now;
                myObject.DeleteDate = DateTime.Now;
                myObject.IsDeleted = true;

                db.Oils.Attach(myObject);
                db.Entry(myObject).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<Oil> Get(Guid oilGuid)
        {
            using (var db = new StationContext())
                return await db.Oils.FindAsync(oilGuid);
        }

        public bool Post(OilDelivery myDelivery)
        {
            using (var db = new StationContext())
            {
                if (myDelivery.OilDeliveryGuid == Guid.Empty) myDelivery.OilDeliveryGuid = Guid.NewGuid();

                myDelivery.DateAdded = DateTime.Now;
                myDelivery.LastEditDate = DateTime.Now;

                db.OilDeliveries.Add(myDelivery);
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(OilDelivery myDelivery)
        {
            using (var db = new StationContext())
            {
                myDelivery.LastEditDate = DateTime.Now;

                db.OilDeliveries.Attach(myDelivery);
                db.Entry(myDelivery).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public async Task<bool> DeleteDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
            {
                var myObject = await db.OilDeliveries.FindAsync(deliveryGuid);

                if (myObject == null) throw new InvalidOperationException("DELIVERY_NOT_FOUND");

                myObject.LastEditDate = DateTime.Now;
                myObject.DeleteDate = DateTime.Now;
                myObject.IsDeleted = true;

                db.OilDeliveries.Attach(myObject);
                db.Entry(myObject).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        public OilDelivery GetDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
                return db.OilDeliveries.Find(deliveryGuid);
        }

        public List<Oil> GetOils(List<Guid> oilsGuids)
        {
            using (var db = new StationContext())
                return db.Oils.Where(o => oilsGuids.Contains(o.OilGuid)).ToList();
        }

        public async Task<List<Oil>> GetOils()
        {
            return await Task.Run(() =>
            {
                using (var db = new StationContext())
                    return db.Oils.ToList();
            });
        }

        public async Task<bool> Post(List<OilPrelevement> oilPrelevements, DateTime fromDate)
        {
            using (var db = new StationContext())
            {
                foreach (var prelevement in oilPrelevements)
                {
                    if (prelevement.OilPrelevementGuid == Guid.Empty) prelevement.OilPrelevementGuid = Guid.NewGuid();
                    prelevement.CurrentPrice = db.Oils.Find(prelevement.OilGuid).CurrentUnitPrice;

                    var lastOne = StaticGetLastPrelevement(prelevement.OilGuid);

                    prelevement.Result = - prelevement.Meter + lastOne.Meter + GetDeliveries(prelevement.OilGuid, lastOne.DatePrelevement.GetValueOrDefault(), fromDate);

                    if (prelevement.Result < 0)
                        throw new CoolException(
                            $"{db.Oils.Find(prelevement.OilGuid).Libel} : Le stock restant ne doit pas etre Supérieur a " + lastOne.Meter + GetDeliveries(prelevement.OilGuid, lastOne.DatePrelevement.GetValueOrDefault(), fromDate));

                    if (prelevement.Result > StaticGetOilBalance(prelevement.OilGuid))
                        throw new CoolException(
                            $"{db.Oils.Find(prelevement.OilGuid).Libel}: Le stock restant ne doit pas etre Supérieur a " + StaticGetOilBalance(prelevement.OilGuid));
                    
                    prelevement.DatePrelevement = fromDate;
                    prelevement.DateAdded = DateTime.Now;
                    prelevement.LastEditDate = DateTime.Now;

                    db.OilPrelevements.Add(prelevement);                   
                }                                                  
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> DeletePrelevement(Guid oilPrelevementGuid)
        {
            using (var db = new StationContext())
            {
                var myObject = await db.OilPrelevements.FindAsync(oilPrelevementGuid);

                if (myObject == null) throw new InvalidOperationException("PRELEVEMENT_NOT_FOUND");

                myObject.LastEditDate = DateTime.Now;
                myObject.DeleteDate = DateTime.Now;
                myObject.IsDeleted = true;

                db.OilPrelevements.Attach(myObject);
                db.Entry(myObject).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> ChangePrice(Guid oilGuid, double newPrice)
        {
            using (var db = new StationContext())
            {
                var myOil = db.Oils.Find(oilGuid);

                myOil.CurrentUnitPrice = newPrice;
                myOil.LastPriceUpdate = DateTime.Now;

                myOil.LastEditDate = DateTime.Now;

                db.Oils.Attach(myOil);
                db.Entry(myOil).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }


        #endregion





        #region Views



        public async Task<double> GetTotalDeliveryCost(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate) 
            => (await GetDeliveries(oilsGuids, fromDate, toDate)).Sum(p => p.Cost);

        public async Task<int> GetTotalDelivery(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate) 
            => (await GetDeliveries(oilsGuids, fromDate, toDate)).Sum(p => p.QuantityDelivered);

        public async Task<double> GetSold(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate) 
            => (await GetPrelevemnts(oilsGuids, fromDate, toDate)).Sum(p => p.Result*p.CurrentPrice);

        public async Task<int> GetGallonsSold(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate) 
            => (await GetPrelevemnts(oilsGuids, fromDate, toDate)).Sum(p => p.Result);

        public int GetDeliveries(Guid oilGuid, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext())
                try
                {
                    return db.Oils.Find(oilGuid)?.Deliveries
                                  .Where(d => d.DeliveryDate >= fromDate && d.DeliveryDate <= toDate && !d.IsDeleted)
                                  .Sum(s => s.QuantityDelivered) ?? 0;
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
        }

        public async Task<List<OilPrelevCard>> GetPrelevCards(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()){
                    var prelevements = new List<OilPrelevement>();
                    foreach (var oil in db.Oils.Where(f => oilsGuids.Contains(f.OilGuid)))
                        prelevements.AddRange( oil.Prelevements.Where(
                               p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate &&
                                    p.DatePrelevement.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                    return prelevements.OrderByDescending(p => p.DatePrelevement)
                            .Select(p => new OilPrelevCard(p))
                            .ToList();
                }});
        }

        public int GetOilBalance(Guid oilGuid) => StaticGetOilBalance(oilGuid);

        public async Task<List<OilCard>> GetOilsCards()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                    return db.Oils.Where(o=>!o.IsDeleted).ToList().OrderByDescending(c => c.DateAdded).Select(c => new OilCard(c)).ToList();
            });
        }

        public List<string> GetTypes()
        {
            using (var db = new StationContext())
                return
                    db.Oils.Where(p => !string.IsNullOrEmpty(p.TypeOil))
                        .Select(p => p.TypeOil.ToLower())
                        .Distinct()
                        .ToList()
                        .Select(s => s.Titleize())
                        .ToList();
        }


        #endregion




        #region Analytic

        public async Task<List<KeyValuePair<DateTime, double>>> GetPrices(Guid oilGuid, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext())
                return (await db.Oils.FindAsync(oilGuid)).Prelevements.OrderBy(p=> p.DatePrelevement).Select(p=> new KeyValuePair<DateTime, double>(p.DatePrelevement.GetValueOrDefault(), p.CurrentPrice)).ToList();                        
        }


        public async Task<List<KeyValuePair<DateTime, double>>> GetMonthlySales(List<Guid> oilGuids, DateTime fromDate, DateTime toDate)
        {
            if (!oilGuids.Any())
                oilGuids = (await GetOils()).Select(o => o.OilGuid).ToList();
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await GetGallonsSold(oilGuids, date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;           
        }


        public async Task<List<KeyValuePair<DateTime, double>>> GetMonthlyIncome(List<Guid> oilGuids, DateTime fromDate, DateTime toDate)
        {
            if (!oilGuids.Any())
                oilGuids = (await GetOils()).Select(o => o.OilGuid).ToList();
           
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await GetSold(oilGuids, date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;           
        }

        #endregion




        #region Internal Static

        public static async Task<Oil> StaticGet(Guid oilGuid)
        {
            using (var db = new StationContext())
                return await db.Oils.FindAsync(oilGuid);
        }

        internal async static Task<double> GetSold(DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() =>{
                    using (var db = new StationContext())
                        return db.OilPrelevements.Any(
                                p => p.DatePrelevement >= fromDate && p.DatePrelevement <= toDate && !p.IsDeleted)
                                ? db.OilPrelevements.Where(
                                      p => p.DatePrelevement >= fromDate &&
                                      p.DatePrelevement <= toDate && !p.IsDeleted)
                                    .Sum(p => p.Result*p.CurrentPrice)
                                : 0;
            });
        }

        internal async static Task<List<OilPrelevement>> GetPrelevemnts(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate)
        {
           return await Task.Run(() => {
                using (var db = new StationContext()) {
                var prelevements = new List<OilPrelevement>();
                foreach (var oilGuid in oilsGuids)
                    prelevements.AddRange(db.Oils.Find(oilGuid)
                            .Prelevements.Where(
                                p => p.DatePrelevement >= fromDate &&
                                     p.DatePrelevement <= toDate && !p.IsDeleted));
                return prelevements;
                }});
        }

        internal async static Task<List<OilDelivery>> GetDeliveries(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                {
                    var deliveries = new List<OilDelivery>();

                    foreach (var oilGuid in oilsGuids)
                        deliveries.AddRange(
                            db.Oils.Find(oilGuid)
                                .Deliveries.Where(
                                    p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate &&
                                         p.DeliveryDate.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                    return deliveries;
                }});
        }

        internal static OilPrelevement StaticGetLastPrelevement(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid).Prelevements.Where(p =>!p.IsDeleted).OrderByDescending(p => p.DatePrelevement).FirstOrDefault() ??
                    new OilPrelevement { Meter = db.Oils.Find(oilGuid).InitialStock};
        }

        public IEnumerable GetOilDeliveries(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid)?.Deliveries.Where(d =>!d.IsDeleted).OrderByDescending(s => s.DateAdded).ToList().Select(s => new OilDeliveryCard(s)).ToList();
        }

        public async Task<List<OilDeliveryCard>> GetOilDeliveries(List<Guid> oilsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                {
                    var deliveries = new List<OilDelivery>();
                    foreach (var oil in db.Oils.Where(f => oilsGuids.Contains(f.OilGuid)))
                        deliveries.AddRange(oil.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                    return deliveries.OrderByDescending(p => p.DeliveryDate).Select(p => new OilDeliveryCard(p)).ToList();
                }});
        }

        internal static int StaticGetOilBalance(Guid oilGuid)
        {
            using (var db = new StationContext())
                try
                {
                    var stocks = db.Oils.Find(oilGuid).InitialStock + db.Oils.Find(oilGuid)?.Deliveries.Where(d =>!d.IsDeleted).Sum(s => s.QuantityDelivered) ?? 0;

                    var solds = db.Oils.Find(oilGuid)?.Prelevements.Where(p =>!p.IsDeleted).Sum(p => p.Result) ?? 0;

                    return (stocks - solds);
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
        }

        #endregion

        
    }
}
