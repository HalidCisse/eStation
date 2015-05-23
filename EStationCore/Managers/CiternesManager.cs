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

        public bool PostStock(FuelStock fuelStock)
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

        public bool PutStock(FuelStock myStock)
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




        #region Helpers


        public List<string> GetSuppliers()
        {
            using (var db = new StationContext())
            {
                var deps = (from s in db.FuelStocks.ToList() where !string.IsNullOrEmpty(s.Supplier) select s.Supplier).Distinct().ToList();

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
                return db.Citernes.ToList().Select(c => new CiterneCard(c)).ToList();
        }


        public IEnumerable GetCiternes()
        {
            using (var db = new StationContext())
                return db.Citernes.ToList();
        }


        public IEnumerable<StockCard> GetCiterneStocks(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid)?.Stocks.ToList().Select(s => new StockCard(s)).ToList();
        }



        #endregion



        #region Internal Static

        internal static double GetCiterneStock(Guid citerneGuid)
        {
            return 333;
        }

        #endregion

        
    }
}
