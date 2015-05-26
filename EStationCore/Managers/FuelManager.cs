using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CLib;
using EStationCore.Model;
using EStationCore.Model.Fuel.Entity;
using EStationCore.Model.Fuel.Views;


namespace EStationCore.Managers
{
    public class FuelManager
    {

        #region CRUD


        public bool Post(Fuel myFuel)
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

                return db.SaveChanges() > 0;
            }
        }        

        public bool Put(Fuel myFuel)
        {
            using (var db = new StationContext())
            {
                myFuel.LastEditDate = DateTime.Now;

                db.Fuels.Attach(myFuel);
                db.Entry(myFuel).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(Guid fuelGuid)
        {
            using (var db = new StationContext())
            {
                db.Fuels.Remove(db.Fuels.Find(fuelGuid));
                return db.SaveChanges() > 0;
            }
        }

        public Fuel Get(Guid fuelGuid)
        {
            using (var db = new StationContext())
                return db.Fuels.Find(fuelGuid);
        }


        #endregion



        #region Views




        public double GetTotalDeliveryLiter(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext()){
                var stocks = new List<FuelDelivery>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        stocks.AddRange(citerne.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate));
                return stocks.Sum(p => p.QuantityDelivered);
            }
        }


        public double GetTotalDeliveryCost(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext()){
                var stocks = new List<FuelDelivery>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        stocks.AddRange(citerne.Deliveries.Where(p => p.DeliveryDate.GetValueOrDefault().Date >= fromDate && p.DeliveryDate.GetValueOrDefault().Date <= toDate));
                return stocks.Sum(p => p.Cost);
            }
        }


        public double GetSold(List<Guid> fuelsGuids, DateTime fromDate, DateTime toDate)
        {
            using (var db = new StationContext())
            {
                var prelevements = new List<Prelevement>();
                foreach (var citernes in db.Fuels.Where(f => fuelsGuids.Contains(f.FuelGuid)).Select(d => d.Citernes))
                    foreach (var citerne in citernes)
                        prelevements.AddRange(citerne.Prelevements.Where(p => p.DatePrelevement.GetValueOrDefault().Date >= fromDate && p.DatePrelevement.GetValueOrDefault().Date <= toDate));
                return prelevements.Sum(p => p.MeterE * p.ActualPrice);
            }
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


        public IEnumerable<FuelCard> GetFuelCards()
        {
            using (var db = new StationContext())
                return db.Fuels.ToList().Select(f => new FuelCard(f)).ToList();
        }


        public IEnumerable GetFuels()
        {
            using (var db = new StationContext())
                return db.Fuels.ToList();
        }


        #endregion



        #region Static Internal


        internal static double StaticGetFuelBalance(Guid fuelGuid)
        {
            using (var db = new StationContext())
            {
                double diff;
                try
                {
                    var stocks = db.Fuels.Find(fuelGuid)?.Citernes.Select(c=> c.Deliveries).Sum(s => s.Sum(l=> l.QuantityDelivered));
                    if (stocks == null)
                        return 0;

                    var prelevs = db.Fuels.Find(fuelGuid)?.Citernes.Select(p=> p.Prelevements).Sum(p => p.Sum(v=> v.MeterE));
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
        }


        internal static double GetFuelCurrentPrice(Guid fuelGuid)
        {
            using (var db = new StationContext())
                return db.Fuels.Find(fuelGuid).Prices.Where(p => p.FromDate <= DateTime.Now).OrderByDescending(p => p.FromDate).First().ActualPrice;
        }


        internal static double GetFuelStock(Guid fuelGuid)
        {
            return 333;
        }


        #endregion

        
    }
}
