using System;
using System.CodeDom;
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
    public class CiternesManager
    {


        #region CRUD

        public bool Post(Citerne myCiternes)
        {
            using (var db = new StationContext())
            {                
                if (myCiternes.CiterneGuid == Guid.Empty) myCiternes.CiterneGuid = Guid.NewGuid();

                myCiternes.DateAdded = DateTime.Now;
                myCiternes.LastEditDate = DateTime.Now;

                db.Citernes.Add(myCiternes);
                return db.SaveChanges() > 0;
            }
        }       

        public bool Put(Citerne myCiternes)
        {
            using (var db = new StationContext())
            {
                myCiternes.LastEditDate = DateTime.Now;

                db.Citernes.Attach(myCiternes);
                db.Entry(myCiternes).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                db.Citernes.Remove(db.Citernes.Find(citerneGuid));
                return db.SaveChanges() > 0;
            }
        }

        public Citerne Get(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid);
        }

        public FuelStock GetStock(Guid stockGuid)
        {
            using (var db = new StationContext())
                return db.FuelStocks.Find(stockGuid);
        }

        public bool Post(FuelStock fuelStock)
        {
            using (var db = new StationContext())
            {
                if (fuelStock.FuelStockGuid == Guid.Empty) fuelStock.FuelStockGuid = Guid.NewGuid();

                fuelStock.DateAdded = DateTime.Now;
                fuelStock.LastEditDate = DateTime.Now;

                db.FuelStocks.Add(fuelStock);
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(FuelStock myStock)
        {
            using (var db = new StationContext())
            {
                myStock.LastEditDate = DateTime.Now;

                db.FuelStocks.Attach(myStock);
                db.Entry(myStock).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }



        #endregion




        #region Views

        public double GetCiterneFuelBalance(Guid citerneGuid) => StaticGetCiterneFuelBalance(citerneGuid);


        public List<string> GetSuppliers()
        {
            using (var db = new StationContext())
            {
                var deps = (from s in db.FuelStocks.OrderByDescending(f=> f.DateAdded).ToList() where !string.IsNullOrEmpty(s.Supplier) select s.Supplier).Distinct().ToList();

                return !deps.Any()
                    ? new List<string>
                    {
                        "Winxo",                        
                    }
                    : deps;
            }
        }


        public IEnumerable<CiterneCard> GetCiternesCards()
        {
            using (var db = new StationContext())
                return db.Citernes.ToList().OrderByDescending(c=> c.DateAdded).Select(c => new CiterneCard(c)).ToList();
        }


        public IEnumerable GetCiternes()
        {
            using (var db = new StationContext())
                return db.Citernes.OrderByDescending(c=> c.DateAdded).ToList();
        }


        public IEnumerable<StockCard> GetCiterneStocks(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid)?.Stocks.OrderByDescending(s=> s.DateAdded).ToList().Select(s => new StockCard(s)).ToList();
        }



        #endregion



        #region Internal Static


        internal static double GetCiterneStock(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                var stocks = db.Citernes.Find(citerneGuid)?.Stocks.Sum(s => s.Quantity);
                if (stocks == null)
                    return 0;
                return (double) stocks;                
            }
        }


        internal static double StaticGetCiterneFuelBalance(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                double diff;
                try
                {
                    var stocks = db.Citernes.Find(citerneGuid)?.Stocks.Sum(s => s.Quantity);
                    if (stocks == null)
                        return 0;

                    var prelevs = db.Citernes.Find(citerneGuid).Prelevements.Sum(p => p.MeterE);

                    diff = (double) (stocks - prelevs);
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
                return diff < 0 ? 0 : diff;               
            }
        }


        internal static double GetCiternePrelevement(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                var sum = db.Citernes.Find(citerneGuid)?.Pompes.Select(p=> p.Prelevements.Select(v=> v.Meter)).Select(s=> s.Sum());
                if (sum != null)
                    return sum.Sum();
            }
            return 0;
        }


        #endregion


    }
}
