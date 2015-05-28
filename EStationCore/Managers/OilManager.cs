using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CLib;
using EStationCore.Model;
using EStationCore.Model.Oil.Entity;
using EStationCore.Model.Oil.Views;
using Humanizer;

namespace EStationCore.Managers
{
    public class OilManager
    {


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

        public bool Delete(Guid oilGuid)
        {
            using (var db = new StationContext())
            {
                db.Oils.Remove(db.Oils.Find(oilGuid));
                return db.SaveChanges() > 0;
            }
        }

        public Oil Get(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid);
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

        public bool DeleteDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
            {
                db.OilDeliveries.Remove(db.OilDeliveries.Find(deliveryGuid));
                return db.SaveChanges() > 0;
            }
        }

        public OilDelivery GetDelivery(Guid deliveryGuid)
        {
            using (var db = new StationContext())
                return db.OilDeliveries.Find(deliveryGuid);
        }


        #endregion




        #region Views








        public int GetOilBalance(Guid oilGuid) => StaticGetOilBalance(oilGuid);

        public IEnumerable<OilCard> GetOilsCards()
        {
            using (var db = new StationContext())
                return db.Oils.ToList().OrderByDescending(c => c.DateAdded).Select(c => new OilCard(c)).ToList();
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







        #region Internal Static




        public IEnumerable GetOilDeliveries(Guid oilGuid)
        {
            using (var db = new StationContext())
                return db.Oils.Find(oilGuid)?.Deliveries.OrderByDescending(s => s.DateAdded).ToList().Select(s => new OilDeliveryCard(s)).ToList();
        }



        internal static int StaticGetOilBalance(Guid oilGuid)
        {
            using (var db = new StationContext())
                try
                {
                    var stocks = db.Oils.Find(oilGuid)?.Deliveries.Sum(s => s.QuantityDelivered) ?? 0;

                    //var prelevs = db.Oils.Find(oilGuid)?.Prelevements.Sum(p => p.MeterE) ?? 0;

                    return (int)(stocks);
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
