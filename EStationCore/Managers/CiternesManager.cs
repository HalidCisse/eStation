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

        public FuelDelivery GetStock(Guid stockGuid)
        {
            using (var db = new StationContext())
                return db.FuelDeliverys.Find(stockGuid);
        }

        public bool Post(FuelDelivery fuelDelivery)
        {
            using (var db = new StationContext())
            {
                if (fuelDelivery.FuelDeliveryGuid == Guid.Empty) fuelDelivery.FuelDeliveryGuid = Guid.NewGuid();

                fuelDelivery.DateAdded = DateTime.Now;
                fuelDelivery.LastEditDate = DateTime.Now;

                db.FuelDeliverys.Add(fuelDelivery);
                return db.SaveChanges() > 0;
            }
        }

        public bool Put(FuelDelivery myDelivery)
        {
            using (var db = new StationContext())
            {
                myDelivery.LastEditDate = DateTime.Now;

                db.FuelDeliverys.Attach(myDelivery);
                db.Entry(myDelivery).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }



        #endregion




        #region Views



        public IEnumerable<FuelCard> GetFuelCards()
        {
            using (var db = new StationContext())
                return db.Fuels.ToList().OrderByDescending(c => c.DateAdded).Select(c => new FuelCard(c)).ToList();
        }


        public double GetCiterneFuelBalance(Guid citerneGuid) => StaticGetCiterneFuelBalance(citerneGuid);


        public List<string> GetSuppliers()
        {
            using (var db = new StationContext())
            {
                var deps = (from s in db.FuelDeliverys.OrderByDescending(f=> f.DateAdded).ToList() where !string.IsNullOrEmpty(s.Supplier) select s.Supplier).Distinct().ToList();

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


        public IEnumerable<Citerne> GetCiternes()
        {
            using (var db = new StationContext())
                return db.Citernes.OrderByDescending(c=> c.DateAdded).ToList();
        }


        public IEnumerable<StockCard> GetCiterneStocks(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid)?.Deliveries.OrderByDescending(s=> s.DateAdded).ToList().Select(s => new StockCard(s)).ToList();
        }



        #endregion



        #region Internal Static


        


        internal static double StaticGetCiterneFuelBalance(Guid citerneGuid)
        {
            using (var db = new StationContext()){
                try
                {
                    var stocks = db.Citernes.Find(citerneGuid)?.Deliveries.Sum(s => s.QuantityDelivered) ?? 0;

                    var prelevs = db.Citernes.Find(citerneGuid)?.Prelevements.Sum(p => p.MeterE) ?? 0;

                    return stocks - prelevs;
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
            }
        }


        internal static double GetCiterneStock(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                var stocks = db.Citernes.Find(citerneGuid)?.Deliveries.Sum(s => s.QuantityDelivered);
                if (stocks == null)
                    return 0;
                return (double)stocks;
            }
        }


        internal static double GetCiternePrelevement(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid)?
                         .Pompes.Select(p => p.Prelevements.Select(v => v.MeterE))
                         .Select(s => s.Sum())
                         .Sum() ?? 0;
        }


        #endregion

        
    }
}
