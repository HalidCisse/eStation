using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using eStationCore.IManagers;
using eStationCore.Model;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Model.Fuel.Views;

namespace eStationCore.Store.SqlServer
{
    public class FuelManager : IFuelManager
    {
        private readonly StationContext Db;

        public FuelManager(StationContext stationContext)
        {
            Db = stationContext;
        }

        #region CRUD


        public async Task<bool> Post(Fuel myFuel)
        {
            using (var db = new StationContext())
            {              
                if (myFuel.FuelGuid == Guid.Empty) myFuel.FuelGuid = Guid.NewGuid();

                myFuel.DateAdded = DateTime.Now;
                myFuel.LastEditDate = DateTime.Now;

                db.Fuels.Add(myFuel);
                if (!myFuel.Prices.Any()) return db.SaveChanges() > 0;

                if (myFuel.Prices.First().PriceGuid == Guid.Empty) myFuel.Prices.First().PriceGuid = Guid.NewGuid();
                myFuel.Prices.First().ProductGuid = myFuel.FuelGuid;
                myFuel.Prices.First().FromDate = DateTime.Now;
                db.Set<Price>().Add(myFuel.Prices.First());

                return await db.SaveChangesAsync() > 0;
            }
        }        

        public async Task<bool> Put(Fuel myFuel)
        {
            using (var db = new StationContext())
            {
                myFuel.LastEditDate = DateTime.Now;

                db.Fuels.Attach(myFuel);
                db.Entry(myFuel).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> Delete(Guid fuelGuid)
        {
            using (var db = new StationContext())
            {
                var myObject = await db.Fuels.FindAsync(fuelGuid);

                if (myObject == null) throw new InvalidOperationException("FUEL_NOT_FOUND");

                myObject.LastEditDate = DateTime.Now;
                myObject.IsDeleted = true;

                db.Fuels.Attach(myObject);
                db.Entry(myObject).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }
        
        public async Task<Fuel> Get(Guid fuelGuid)
        {
            using (var db = new StationContext())
                return await db.Fuels.FindAsync(fuelGuid);
        }


        public async Task<bool> DeleteDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
            {
                var myObject = await db.FuelDeliverys.FindAsync(deliveryGuid);

                if (myObject == null) throw new InvalidOperationException("DELIVERY_NOT_FOUND");

                myObject.LastEditDate = DateTime.Now;
                myObject.DeleteDate = DateTime.Now;
                myObject.IsDeleted = true;

                db.FuelDeliverys.Attach(myObject);
                db.Entry(myObject).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        

        #endregion



        #region Functions


        public async Task<double> GetTotalDeliveryLiter(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()){
                var stocks = new List<FuelDelivery>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes.Where(c => !c.IsDeleted)))
                    foreach (var citerne in citernes)
                        stocks.AddRange(citerne.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                return stocks.Sum(p => p.QuantityDelivered);
            }
            });
        }


        public async Task<double> GetTotalDeliveryCost(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()){
                var stocks = new List<FuelDelivery>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes.Where(c=> !c.IsDeleted)))
                    foreach (var citerne in citernes)
                        stocks.AddRange(citerne.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                return stocks.Sum(p => p.Cost);
            }
            });
        }


        public async Task<double> GetSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
            {
                var prelevements = new List<FuelPrelevement>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes.Where(c => !c.IsDeleted)))
                    foreach (var citerne in citernes)
                        prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                return prelevements.Sum(p => p.Result * p.CurrentPrice);
            }
            });
        }


        public async Task<double> GetLiterSoldAsync(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                {
                    var prelevements = new List<FuelPrelevement>();
                    foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes.Where(c => !c.IsDeleted)))
                        foreach (var citerne in citernes)
                            prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                    return prelevements.Sum(p => p.Result);
                }
            });
        }


        public double GetLiterSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {            
                using (var db = new StationContext())
                {
                    var prelevements = new List<FuelPrelevement>();
                    foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes.Where(c => !c.IsDeleted)))
                        foreach (var citerne in citernes)
                            prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate && !p.IsDeleted));
                    return prelevements.Sum(p => p.Result);
                }           
        }


        public async Task<List<FuelCard>> GetFuelCards()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Fuels.Where(f=> !f.IsDeleted).ToList().Select(f => new FuelCard(f)).ToList();
            });
        }


        public async Task<List<Fuel>> GetFuels(DateTime date = default(DateTime))
        {
            return await Task.Run(() =>
            {
                using (var db = new StationContext())
                    return date == default(DateTime)
                        ? db.Fuels.Where(f => !f.IsDeleted).ToList()
                        : db.Fuels.Where(f => !f.IsDeleted || f.DeleteDate < date).ToList();
            });
        }


        public async Task<List<Fuel>> GetFuels(List<Guid> fuelsGuids)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Fuels.Where(o => fuelsGuids.Contains(o.FuelGuid)).ToList();
            });
        }


        public async Task<double> GetFuelActualPrice(Guid fuelGuid)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Fuels.FindAsync(fuelGuid).Result.Prices.Where(p => p.FromDate <= DateTime.Now).OrderByDescending(p => p.FromDate).First().ActualPrice;
            });
        }

        #endregion



        #region Analytic


        public async Task<List<KeyValuePair<DateTime, double>>> GetPrices(Guid fuelGuid, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext())
                return (await db.Fuels.FindAsync(fuelGuid)).Prices.OrderBy(p=>p.FromDate).Select(p => new KeyValuePair<DateTime, double>(p.FromDate.GetValueOrDefault(), p.ActualPrice)).ToList();
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetMonthlySales(List<Guid> fuelGuids, DateTime fromDate, DateTime toDate)
        {
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await GetLiterSoldAsync(fuelGuids, date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;            
        }

        public async Task<List<KeyValuePair<DateTime, double>>> GetMonthlyIncome(DateTime fromDate, DateTime toDate)
        {
            var points = new List<KeyValuePair<DateTime, double>>();
            foreach (var date in DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1)))
                points.Add(new KeyValuePair<DateTime, double>(date,
                    await GetSold(date.Date, date.Date.AddMonths(1).AddDays(-1))));
            return points;            
        }

        #endregion



        #region Static Internal




        internal async static Task<double> GetSold(DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                    using (var db = new StationContext())
                        return db.FuelPrelevements.Any(
                                    p =>
                                    p.DatePrelevement >= fromDate && p.DatePrelevement <= toDate && !p.IsDeleted )
                                ? db.FuelPrelevements.Where(
                                    p =>
                                    p.DatePrelevement >= fromDate &&
                                    p.DatePrelevement <= toDate && !p.IsDeleted)
                                    .Sum(p => p.Result*p.CurrentPrice)
                                : 0;
            });
        }

        internal async static Task<double> StaticGetFuelBalance(Guid fuelGuid)
        {
            return await Task.Run(async () => {
                using (var db = new StationContext())
            {
                double diff;
                try
                {
                    var stocks = (await db.Fuels.FindAsync(fuelGuid))?.Citernes.Where(c=>!c.IsDeleted).Select(c=> c.Deliveries.Where(d=>!d.IsDeleted)).Sum(s => s.Sum(l=> l.QuantityDelivered));
                    if (stocks == null)
                        return 0;

                    var prelevs = (await db.Fuels.FindAsync(fuelGuid))?.Citernes.Where(c => !c.IsDeleted).Select(p=> p.Prelevements.Where(l=>!l.IsDeleted)).Sum(p => p.Sum(v=> v.Result));
                    if (prelevs == null)
                        return 0;

                    diff = (double)(stocks - prelevs);
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
                return diff < 0 ? 0 : diff;
            }
            });
        }

        internal static async Task<double> GetFuelCurrentPrice(Guid fuelGuid) => (await GetFuelLastPrice(fuelGuid)).ActualPrice;

        internal async static Task<Price> GetFuelLastPrice(Guid fuelGuid)
        {
            return await Task.Run(async () => {
                using (var db = new StationContext())
                return (await db.Fuels.FindAsync(fuelGuid)).Prices.Where(p => p.FromDate <= DateTime.Now).OrderByDescending(p => p.FromDate).First();
            });
        }


        #endregion

        
    }
}
