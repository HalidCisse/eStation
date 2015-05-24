using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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



        public double GetTotalSold(List<Guid> citernesGuid, DateTime fromDate, DateTime toDate)
        {
            return 333;
        }


        public double GetTotalLiterSold(List<Guid> citernesGuid, DateTime fromDate, DateTime toDate)
        {
            return 333;
        }


        public double GetFuelBalance(Guid fuelGuid)
        {           
            using (var db = new StationContext())
            {
                var prelevs = db.Fuels.Find(fuelGuid).Citernes.Select(c => c.Prelevements).ToList().Select(p=> p.Sum(s=> s.MeterE)).Sum();
                var stocks = db.Fuels.Find(fuelGuid).Citernes.Select(c => c.Stocks).ToList().Select(s=> s.Sum(d=> d.Quantity)).Sum();

                var diff = stocks - prelevs;
                return diff < 0 ? 0 : diff;
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
