using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CLib;
using EStationCore.Model;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;

namespace EStationCore.Managers
{
    public class FuelManager
    {

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
                db.Fuels.Remove(db.Fuels.Find(fuelGuid));
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<Fuel> Get(Guid fuelGuid)
        {
            using (var db = new StationContext())
                return await db.Fuels.FindAsync(fuelGuid);
        }


        #endregion



        #region Views

        
        public async Task<double> GetTotalDeliveryLiter(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()){
                var stocks = new List<FuelDelivery>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        stocks.AddRange(citerne.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate));
                return stocks.Sum(p => p.QuantityDelivered);
            }
            });
        }


        public async Task<double> GetTotalDeliveryCost(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()){
                var stocks = new List<FuelDelivery>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        stocks.AddRange(citerne.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate));
                return stocks.Sum(p => p.Cost);
            }
            });
        }


        public async Task<double> GetSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
            {
                var prelevements = new List<Prelevement>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate));
                return prelevements.Sum(p => p.MeterE * p.ActualPrice);
            }
            });
        }


        public async Task<double> GetLiterSoldAsync(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                {
                    var prelevements = new List<Prelevement>();
                    foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                        foreach (var citerne in citernes)
                            prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate));
                    return prelevements.Sum(p => p.MeterE);
                }
            });
        }

        public double GetLiterSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {            
                using (var db = new StationContext())
                {
                    var prelevements = new List<Prelevement>();
                    foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                        foreach (var citerne in citernes)
                            prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate));
                    return prelevements.Sum(p => p.MeterE);
                }           
        }


        public async Task<List<FuelCard>> GetFuelCards()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Fuels.ToList().Select(f => new FuelCard(f)).ToList();
            });
        }


        public async Task<List<Fuel>> GetFuels() => await Task.Run(() =>{
            using (var db = new StationContext())
                return db.Fuels.ToList();
        });

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
                return (await db.Fuels.FindAsync(fuelGuid)).Prices.Select(p => new KeyValuePair<DateTime, double>(p.FromDate.GetValueOrDefault(), p.ActualPrice)).ToList();
        }

        public IEnumerable<KeyValuePair<string, double>> GetMonthlySales(List<Guid> fuelGuids, DateTime fromDate, DateTime toDate)
                    => (DateTimeHelper.EachMonth(new DateTime(fromDate.Year, fromDate.Month, 1), new DateTime(toDate.Year, toDate.Month, 1))
                        .Select( month => new KeyValuePair<string, double>(month.ToString("MMM-yy"),
                           GetLiterSoldAsync(fuelGuids, month.Date, month.Date.AddMonths(1).AddDays(-1)).Result)));

        
        #endregion



        #region Static Internal


        internal async static Task<double> StaticGetFuelBalance(Guid fuelGuid)
        {
            return await Task.Run(async () => {
                using (var db = new StationContext())
            {
                double diff;
                try
                {
                    var stocks = (await db.Fuels.FindAsync(fuelGuid))?.Citernes.Select(c=> c.Deliveries).Sum(s => s.Sum(l=> l.QuantityDelivered));
                    if (stocks == null)
                        return 0;

                    var prelevs = (await db.Fuels.FindAsync(fuelGuid))?.Citernes.Select(p=> p.Prelevements).Sum(p => p.Sum(v=> v.MeterE));
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

        internal static async Task<double> GetFuelCurrentPrice(Guid fuelGuid) 
            => (await GetFuelLastPrice(fuelGuid)).ActualPrice;

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
